using DevExpress.Mvvm.Native;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Export.Enums;
using Export.Interfaces;
using Export.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using ParagraphAlignment = DevExpress.XtraRichEdit.API.Native.ParagraphAlignment;

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
            _richServer.Document.Sections[^1].Page.PaperKind = PaperKind.A4;

            _printingSystem = new PrintingSystem();
            _link = new PrintableComponentLink(_printingSystem);

            _path = path;
            _nameFile = nameFile;
        }

        public void AddChart(Chart chart)
        {
            // Логика формирования графика

            AddText(new Text("", new SettingText() { TextAligment = Aligment.Center }));

            Image image = chart.GetImage();

            _richServer.Document.Images.Append(DocumentImageSource.FromImage(image));
        }

        public void AddTable(TableModel table)
        {
            // Логика формирования/заполнения таблицы

            _richServer.Document.Paragraphs.Append();

            DocumentRange rangeParagraph = _richServer.Document.Paragraphs[^1].Range;

            ParagraphProperties paragraph = _richServer.Document.BeginUpdateParagraphs(rangeParagraph);

            paragraph.Alignment = table.TableSetting.SettingText.TextAligment.Equals(Aligment.Center) ? ParagraphAlignment.Center
                : table.TableSetting.SettingText.TextAligment.Equals(Aligment.Left) ? ParagraphAlignment.Left
                : table.TableSetting.SettingText.TextAligment.Equals(Aligment.Right) ? ParagraphAlignment.Right
                : ParagraphAlignment.Justify;

           Table tablePdf = _richServer.Document.Tables.Create(_richServer.Document.Selection.Start, table.TableData.Count + 1, table.HeaderTable.Headers.Count, AutoFitBehaviorType.AutoFitToWindow);

            DocumentRange range = _richServer.Document.Tables[^1].Range;
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

            for (int i = 0; i < table.TableData.Count; i++)
            {
                for (int j = 0; j < table.HeaderTable.Headers.Count; j++)
                {
                    _richServer.Document.InsertText(tablePdf[i + 1, j].Range.Start, table.TableData[i][j].ToString());
                }
            }

            tablePdf.EndUpdate();

            #endregion

            _richServer.Document.EndUpdateParagraphs(paragraph);
        }

        public void AddText(Text text)
        {
            // Логика добавления текста

            _richServer.Document.Paragraphs.Append();

            DocumentRange range = _richServer.Document.Paragraphs[^1].Range;

            CharacterProperties titleFormatting = _richServer.Document.BeginUpdateCharacters(range);

            titleFormatting.FontSize = text.SettingText.FontSize;
            titleFormatting.FontName = text.SettingText.FontName;
            titleFormatting.ForeColor = text.SettingText.Color;
            titleFormatting.Bold = text.SettingText.Bold;
            titleFormatting.Italic = text.SettingText.Italic;

            _richServer.Document.AppendText(text.Letter);

            ParagraphProperties paragraphProperties = _richServer.Document.BeginUpdateParagraphs(range);

            paragraphProperties.Alignment = text.SettingText.TextAligment.Equals(Aligment.Center) ? ParagraphAlignment.Center
                : text.SettingText.TextAligment.Equals(Aligment.Left) ? ParagraphAlignment.Left
                : text.SettingText.TextAligment.Equals(Aligment.Right) ? ParagraphAlignment.Right
                : ParagraphAlignment.Justify;

            _richServer.Document.EndUpdateParagraphs(paragraphProperties);

            _richServer.Document.EndUpdateCharacters(titleFormatting);
        }

        public void AddNewPage()
        {
            _richServer.Document.AppendSection();
            _richServer.Document.Sections[^1].Page.PaperKind = PaperKind.A4;
            _richServer.Document.Sections[^1].Page.Landscape = false;

            _richServer.Document.Unit = DevExpress.Office.DocumentUnit.Inch;
        }

        public void OpenPreview()
        {
            _link.Component = _richServer;

            _link.CreateDocument();
            _link.ShowPreviewDialog();
        }

        public void SaveDocument()
        {
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
