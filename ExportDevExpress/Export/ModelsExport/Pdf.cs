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

        private bool _isTableContent { get; set; }

        public Pdf(string path, string nameFile, bool isTableContent = true)
        {
            _richServer = new RichEditDocumentServer();
            _richServer.Document.Sections[0].Page.PaperKind = PaperKind.A4;

            _printingSystem = new PrintingSystem();
            _link = new PrintableComponentLink(_printingSystem);

            _path = path;
            _nameFile = nameFile;

            _isTableContent = isTableContent;

            if (_isTableContent)
            {
                _richServer.Document.AppendSection();
                _richServer.Document.Sections[^1].Page.PaperKind = PaperKind.A4;
            }
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

            _richServer.Document.BeginUpdate();

            var rangePar = _richServer.Document.Paragraphs.Append();

            Table tablePdf = _richServer.Document.Tables.Create(_richServer.Document.Paragraphs[^1].Range.Start, table.TableData.Count + 1, table.HeaderTable.Headers.Count, AutoFitBehaviorType.AutoFitToWindow);

            tablePdf.BeginUpdate();

            tablePdf.Borders.InsideHorizontalBorder.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderInsideSetting.BorderLineStyle;
            tablePdf.Borders.InsideHorizontalBorder.LineThickness = table.TableSetting.TableBorderInsideSetting.LineThickness;
            tablePdf.Borders.InsideVerticalBorder.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderInsideSetting.BorderLineStyle;
            tablePdf.Borders.InsideVerticalBorder.LineThickness = table.TableSetting.TableBorderInsideSetting.LineThickness;

            tablePdf.Borders.Left.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderSetting.BorderLineStyle;
            tablePdf.Borders.Left.LineThickness = table.TableSetting.TableBorderSetting.LineThickness;
            tablePdf.Borders.Right.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderSetting.BorderLineStyle;
            tablePdf.Borders.Right.LineThickness = table.TableSetting.TableBorderSetting.LineThickness;
            tablePdf.Borders.Top.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderSetting.BorderLineStyle;
            tablePdf.Borders.Top.LineThickness = table.TableSetting.TableBorderSetting.LineThickness;
            tablePdf.Borders.Bottom.LineStyle = (TableBorderLineStyle)table.TableSetting.TableBorderSetting.BorderLineStyle;
            tablePdf.Borders.Bottom.LineThickness = table.TableSetting.TableBorderSetting.LineThickness;

            tablePdf.FirstRow.RepeatAsHeaderRow = table.TableSetting.HeaderSetting.RepeatHeaderEveryPage;

            _richServer.Document.EndUpdate();

            #region Заполнение таблицы

            ParagraphProperties paragraphHeaderProperties = _richServer.Document.BeginUpdateParagraphs(tablePdf.FirstRow.Range);

            paragraphHeaderProperties.Alignment = table.TableSetting.HeaderSetting.SettingText.TextAligment.Equals(Aligment.Center) ? ParagraphAlignment.Center
                : table.TableSetting.HeaderSetting.SettingText.TextAligment.Equals(Aligment.Left) ? ParagraphAlignment.Left
                : table.TableSetting.HeaderSetting.SettingText.TextAligment.Equals(Aligment.Right) ? ParagraphAlignment.Right
                : ParagraphAlignment.Justify;

            table.HeaderTable.Headers.ForEach((headerName, i) =>
            {
                DocumentRange insertedText = _richServer.Document.InsertText(tablePdf[0, i].Range.Start, headerName);

                tablePdf[0, i].BackgroundColor = table.TableSetting.HeaderSetting.BackGroundColor;

                CharacterProperties titleFormatting2 = _richServer.Document.BeginUpdateCharacters(insertedText);

                titleFormatting2.Bold = table.TableSetting.HeaderSetting.SettingText.Bold;
                titleFormatting2.Italic = table.TableSetting.HeaderSetting.SettingText.Italic;
                titleFormatting2.ForeColor = table.TableSetting.HeaderSetting.SettingText.Color;
                titleFormatting2.FontSize = table.TableSetting.HeaderSetting.SettingText.FontSize;

                if (!string.IsNullOrEmpty(table.TableSetting.HeaderSetting.SettingText.FontName))
                    titleFormatting2.FontName = table.TableSetting.HeaderSetting.SettingText.FontName;

                _richServer.Document.EndUpdateCharacters(titleFormatting2);
            });

            _richServer.Document.EndUpdateParagraphs(paragraphHeaderProperties);

            for (int i = 0; i < table.TableData.Count; i++)
            {
                for (int j = 0; j < table.HeaderTable.Headers.Count; j++)
                {
                    ParagraphProperties paragraphBodyProperties = _richServer.Document.BeginUpdateParagraphs(tablePdf[i + 1, j].Range);

                    paragraphBodyProperties.Alignment = table.TableSetting.BodySetting.SettingText.TextAligment.Equals(Aligment.Center) ? ParagraphAlignment.Center
                        : table.TableSetting.BodySetting.SettingText.TextAligment.Equals(Aligment.Left) ? ParagraphAlignment.Left
                        : table.TableSetting.BodySetting.SettingText.TextAligment.Equals(Aligment.Right) ? ParagraphAlignment.Right
                        : ParagraphAlignment.Justify;

                    DocumentRange insertedText = _richServer.Document.InsertText(tablePdf[i + 1, j].Range.Start, table.TableData[i][j].ToString());

                    CharacterProperties titleFormatting2 = _richServer.Document.BeginUpdateCharacters(insertedText);

                    titleFormatting2.Bold = table.TableSetting.BodySetting.SettingText.Bold;
                    titleFormatting2.Italic = table.TableSetting.BodySetting.SettingText.Italic;
                    titleFormatting2.ForeColor = table.TableSetting.BodySetting.SettingText.Color;
                    titleFormatting2.FontSize = table.TableSetting.BodySetting.SettingText.FontSize;

                    if (!string.IsNullOrEmpty(table.TableSetting.BodySetting.SettingText.FontName))
                        titleFormatting2.FontName = table.TableSetting.BodySetting.SettingText.FontName;

                    _richServer.Document.EndUpdateCharacters(titleFormatting2);

                    if (table.TableSetting.BodySetting.ColorRow.ColorEveryRow > 0
                        && (i + 1) % table.TableSetting.BodySetting.ColorRow.ColorEveryRow == 0)
                    {
                        tablePdf[i + 1, j].BackgroundColor = table.TableSetting.BodySetting.ColorRow.BackGroundColor;
                    }

                    _richServer.Document.EndUpdateParagraphs(paragraphBodyProperties);
                }
            }

            #endregion
        }

        public void AddText(Text text)
        {
            // Логика добавления текста

            _richServer.Document.BeginUpdate();

            Paragraph range = _richServer.Document.Paragraphs.Insert(_richServer.Document.Sections[^1].Range.End);

            if (text.IsHeader)
                range.OutlineLevel = text.OutLineLevel;
            else
                range.OutlineLevel = 0;

            DocumentRange rangeText = _richServer.Document.InsertText(range.Range.End, text.Letter);

            CharacterProperties titleFormatting = _richServer.Document.BeginUpdateCharacters(rangeText);

            titleFormatting.FontSize = text.SettingText.FontSize;
            titleFormatting.FontName = text.SettingText.FontName;
            titleFormatting.ForeColor = text.SettingText.Color;
            titleFormatting.Bold = text.SettingText.Bold;
            titleFormatting.Italic = text.SettingText.Italic;

            ParagraphProperties paragraphProperties = _richServer.Document.BeginUpdateParagraphs(rangeText);

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
                //.Append(AddPageNumber)
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
            if (_isTableContent)
            {
                _richServer.Document.BeginUpdate();

                _richServer.Document.Fields.Create(_richServer.Document.Sections[0].Range.Start, @"TOC \h \u");

                _richServer.Document.EndUpdate();
                _richServer.Document.Fields.Update();

                //_richServer.Document.UpdateAllFields();
            }
        }

        private void AddPageNumber()
        {
            //_richServer.Document.Sections[1].PageNumbering.FirstPageNumber = 3;
            //_richServer.Document.Sections[1].PageNumbering.ContinueNumbering = true;

            //var footer = _richServer.Document.Sections[1].BeginUpdateFooter();

            //footer.Fields.Create(footer.Range.End, "PAGE");

            //_richServer.Document.Sections[1].EndUpdateFooter(footer);

            //_richServer.Document.UpdateAllFields();
        }
    }
}
