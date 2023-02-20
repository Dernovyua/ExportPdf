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
using System.Linq;
using ParagraphAlignment = DevExpress.XtraRichEdit.API.Native.ParagraphAlignment;

namespace Export.ModelsExport
{
    public class Pdf : IReport
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
            _richServer.Document.Sections[0].Page.PaperKind = PaperKind.A4;

            _richServer.Document.AppendSection();
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

            _richServer.Document.BeginUpdate();

            if (image != null)
                _richServer.Document.Images.Insert(_richServer.Document.Sections[^1].Range.End, DocumentImageSource.FromImage(image));

            _richServer.Document.EndUpdate();
        }

        public void AddTable(TableModel table)
        {
            // Логика формирования/заполнения таблицы

            //_richServer.Document.Paragraphs.Append();

            _richServer.Document.BeginUpdate();

            Paragraph rangeParagraph = _richServer.Document.Paragraphs.Insert(_richServer.Document.Sections[^1].Range.End);

            //DocumentRange rangeParagraph = _richServer.Document.Paragraphs[^1].Range;

            ParagraphProperties paragraph = _richServer.Document.BeginUpdateParagraphs(rangeParagraph.Range);

            paragraph.Alignment = table.TableSetting.SettingText.TextAligment.Equals(Aligment.Center) ? ParagraphAlignment.Center
                : table.TableSetting.SettingText.TextAligment.Equals(Aligment.Left) ? ParagraphAlignment.Left
                : table.TableSetting.SettingText.TextAligment.Equals(Aligment.Right) ? ParagraphAlignment.Right
                : ParagraphAlignment.Justify;

            Table tablePdf = _richServer.Document.Tables.Create(_richServer.Document.Sections[^1].Range.End, table.TableData.Count + 1, table.HeaderTable.Headers.Count, AutoFitBehaviorType.AutoFitToWindow);

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

            //tStyleMain.Alignment = (ParagraphAlignment)table.TableSetting.TableAligment.ParagraphAlignment;

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

            _richServer.Document.EndUpdate();
        }

        public void AddText(Text text)
        {
            // Логика добавления текста

            _richServer.Document.BeginUpdate();

            Paragraph range = _richServer.Document.Paragraphs.Insert(_richServer.Document.Sections[^1].Range.End);

            if (text.IsHeader)
                range.OutlineLevel = 2;
            else
                range.OutlineLevel = 0;

            DocumentRange rangeText = _richServer.Document.InsertText(range.Range.End, text.Letter);

            CharacterProperties titleFormatting = _richServer.Document.BeginUpdateCharacters(range.Range);

            titleFormatting.FontSize = text.SettingText.FontSize;
            titleFormatting.FontName = text.SettingText.FontName;
            titleFormatting.ForeColor = text.SettingText.Color;
            titleFormatting.Bold = text.SettingText.Bold;
            titleFormatting.Italic = text.SettingText.Italic;

            ParagraphProperties paragraphProperties = _richServer.Document.BeginUpdateParagraphs(range.Range);

            paragraphProperties.Alignment = text.SettingText.TextAligment.Equals(Aligment.Center) ? ParagraphAlignment.Center
                : text.SettingText.TextAligment.Equals(Aligment.Left) ? ParagraphAlignment.Left
                : text.SettingText.TextAligment.Equals(Aligment.Right) ? ParagraphAlignment.Right
                : ParagraphAlignment.Justify;

            _richServer.Document.EndUpdateParagraphs(paragraphProperties);

            _richServer.Document.EndUpdateCharacters(titleFormatting);

            #region Добавление ссылки на указанный текст

            if (text.HyperLink != null && !string.IsNullOrEmpty(text.HyperLink.LinkText))
            {
                DocumentRange[] foundRanges = _richServer.Document.FindAll(text.HyperLink.LinkText, SearchOptions.CaseSensitive, rangeText);
                if (foundRanges.Length > 0)
                {
                    _richServer.Document.Hyperlinks.Create(foundRanges[0]);

                    _richServer.Document.Hyperlinks[^1].NavigateUri = text.HyperLink.TargetLink;
                    _richServer.Document.Hyperlinks[^1].ToolTip = text.HyperLink.ToolTip;
                }
            }

            #endregion

            _richServer.Document.EndUpdate();
        }

        public void AddNewPage(string? name = null)
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
            actions = actions
                .Append(AddTOC);

            foreach (var action in actions)
            {
                action();
            }
        }

        /// <summary>
        /// Добавление оглавления
        /// </summary>
        private void AddTOC()
        {
            #region WOOOOORK

            //Document doc = _richServer.Document;

            //var contentSection = doc.Sections[0];
            //var lastSection = doc.Sections[^1];

            //Document document = _richServer.Document;

            //document.BeginUpdate();

            //bool header = false;

            //for (int i = 0; i < 10; i++)
            //{
            //    if (i % 2 == 0)
            //        header = true;
            //    else
            //        header = false;

            //    Paragraph paragraph = document.Paragraphs.Insert(lastSection.Range.Start);

            //    if (header)
            //        paragraph.OutlineLevel = 2;
            //    else
            //        paragraph.OutlineLevel = 0;

            //    document.InsertText(paragraph.Range.End, $"Title {i + 1}");

            //    //Paragraph range = document.Paragraphs.Insert(lastSection.Range.End);
            //    //document.InsertText(range.Range.Start, "The first created table of content");
            //}
            //document.Fields.Create(contentSection.Range.Start, @"TOC \h \u");

            //Paragraph paragraph2 = document.Paragraphs.Insert(lastSection.Range.Start);
            //document.InsertText(paragraph2.Range.Start, "The first created table of content");

            //document.EndUpdate();
            //document.Fields.Update();

            #endregion

            _richServer.Document.BeginUpdate();

            _richServer.Document.Fields.Create(_richServer.Document.Sections[0].Range.Start, @"TOC \h \u");

            _richServer.Document.EndUpdate();
            _richServer.Document.Fields.Update();
        }
    }
}
