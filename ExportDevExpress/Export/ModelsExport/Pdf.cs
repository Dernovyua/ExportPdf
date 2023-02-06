﻿using DevExpress.Mvvm.Native;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Export.Interfaces;
using Export.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Export.ModelsExport
{
    public class Pdf : ISequence
    {
        #region Свойства DevExpress

        /// <summary>
        /// Класс для взаимодействия с файлом
        /// </summary>
        private RichEditDocumentServer _richServer { get; set; }

        #region Необходимо для превью и создания документа

        private PrintingSystem _printingSystem { get; set; }
        private PrintableComponentLink _link { get; set; }

        #endregion

        #endregion

        private string _path { get; set; }
        private string _nameFile { get; set; }

        public Pdf(string path, string nameFile)
        {
            _richServer = new RichEditDocumentServer();

            _printingSystem = new PrintingSystem();
            _link = new PrintableComponentLink(_printingSystem);

            _path = path;
            _nameFile = nameFile;
        }

        public void AddChart(Chart chart)
        {
            // Логика формирования графика

            AddText(new Text(""));

            Image image = chart.CreateImage();

            _richServer.Document.Images.Append(DocumentImageSource.FromImage(image));

            _richServer.Document.AppendText("\n");
        }

        public void AddTable(TableModel table)
        {
            // Логика формирования/заполнения таблицы

            AddText(new Text(""));

            Table tablePdf = _richServer.Document.Tables.Create(_richServer.Document.Selection.Start, table.TableData.Count, table.HeaderTable.Headers.Count, AutoFitBehaviorType.AutoFitToWindow);

            DocumentRange range = _richServer.Document.Tables[_richServer.Document.Tables.Count - 1].Range;
            CharacterProperties titleFormatting = _richServer.Document.BeginUpdateCharacters(range);

            #region Настройки текста в таблице

            titleFormatting.FontSize = table.TableSetting.SettingText.FontSize;
            titleFormatting.FontName = table.TableSetting.SettingText.FontName;
            titleFormatting.ForeColor = table.TableSetting.SettingText.Color;
            titleFormatting.Bold = table.TableSetting.SettingText.Bold;
            titleFormatting.Italic = table.TableSetting.SettingText.Italic;

            #endregion

            _richServer.Document.EndUpdateCharacters(titleFormatting);

            tablePdf.BeginUpdate();

            #region Настройки таблицы

            TableStyle tStyleMain = _richServer.Document.TableStyles.CreateNew();

            tStyleMain.Alignment = (ParagraphAlignment)table.TableSetting.TableAligment.ParagraphAlignment;

            tStyleMain.TableBorders.InsideHorizontalBorder.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderInsideSetting.BorderLineStyle;
            tStyleMain.TableBorders.InsideHorizontalBorder.LineThickness = table.TableSetting.TableBorderInsideSetting.LineThickness;
            tStyleMain.TableBorders.InsideVerticalBorder.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderInsideSetting.BorderLineStyle;
            tStyleMain.TableBorders.InsideVerticalBorder.LineThickness = table.TableSetting.TableBorderInsideSetting.LineThickness;

            tStyleMain.TableBorders.Left.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderSetting.BorderLineStyle;
            tStyleMain.TableBorders.Left.LineThickness = table.TableSetting.TableBorderSetting.LineThickness;
            tStyleMain.TableBorders.Right.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderSetting.BorderLineStyle;
            tStyleMain.TableBorders.Right.LineThickness = table.TableSetting.TableBorderSetting.LineThickness;
            tStyleMain.TableBorders.Top.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderSetting.BorderLineStyle;
            tStyleMain.TableBorders.Top.LineThickness = table.TableSetting.TableBorderSetting.LineThickness;
            tStyleMain.TableBorders.Bottom.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderSetting.BorderLineStyle;
            tStyleMain.TableBorders.Bottom.LineThickness = table.TableSetting.TableBorderSetting.LineThickness;

            _richServer.Document.TableStyles.Add(tStyleMain);

            tablePdf.Style = tStyleMain;

            tablePdf.Rows[0].RepeatAsHeaderRow = table.TableSetting.RepeatHeaderEveryPage;

            #endregion

            #region Заполнение таблицы

            table.HeaderTable.Headers.ForEach((headreName, i) =>
            {
                _richServer.Document.InsertText(tablePdf[0, i].Range.Start, headreName);
            });

            for (int i = 0; i < table.TableData.Count - 1; i++)
            {
                for (int j = 0; j < table.HeaderTable.Headers.Count; j++)
                {
                    _richServer.Document.InsertText(tablePdf[i + 1, j].Range.Start, table.TableData[i][j].ToString());
                }
            }

            tablePdf.EndUpdate();

            #endregion
        }

        public void AddText(Text text)
        {
            // Логика добавления текста

            DocumentRange range = _richServer.Document.Paragraphs[_richServer.Document.Paragraphs.Count - 1].Range;

            CharacterProperties titleFormatting = _richServer.Document.BeginUpdateCharacters(range);

            titleFormatting.FontSize = text.SettingText.FontSize;
            titleFormatting.FontName = text.SettingText.FontName;
            titleFormatting.ForeColor = text.SettingText.Color;
            titleFormatting.Bold = text.SettingText.Bold;
            titleFormatting.Italic = text.SettingText.Italic;

            _richServer.Document.AppendText(text.Letter + "\n");

            _richServer.Document.EndUpdateCharacters(titleFormatting);
        }

        public void AddNewPage()
        {
            _richServer.Document.AppendSection();
        }

        public void OpenPreview()
        {
            _link.Component = _richServer;

            _link.CreateDocument();
            _link.ShowPreviewDialog();
        }

        public void SaveDocument()
        {
            //using (FileStream fs = new FileStream($@"../../{Guid.NewGuid()}.pdf", FileMode.OpenOrCreate))
            using (FileStream fs = new FileStream($@"{_path}/{_nameFile}.pdf", FileMode.OpenOrCreate))
            {
                _link.Component = _richServer;

                _link.CreateDocument();
                _link.ExportToPdf(fs);
            }
        }

        /// <summary>
        /// Вызов методов в нужной последовательности (который заложит пользователь при добавлении в список)
        /// </summary>
        /// <param name="actions">Список методов</param>
        public void GetCallSequenceMethods(IEnumerable<Action> actions)
        {
            foreach (var action in actions)
            {
                action();
            }
        }
    }
}
