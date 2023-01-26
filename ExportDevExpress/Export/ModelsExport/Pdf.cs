using DevExpress.Mvvm.Native;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Export.Interfaces;
using Export.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;

namespace Export.ModelsExport
{
    public class Pdf : ISequence
    {
        // Здесь будут еще свойства от DevExpress

        #region Свойства DevExpress

        private RichEditDocumentServer _richServer { get; set; }

        private PrintingSystem _printingSystem { get; set; }
        private PrintableComponentLink _link { get; set; }

        public Pdf()
        {
            _richServer = new RichEditDocumentServer();

            _printingSystem = new PrintingSystem();
            _link = new PrintableComponentLink(_printingSystem);

            #region Настройки файла

            _richServer.Document.Sections[0].Page.PaperKind = PaperKind.A4;

            #endregion
        }

        #endregion
        public void AddChart(Chart chart)
        {
            // Логика формирования графика

            Image image = chart.CreateImage();

            _richServer.Document.Images.Append(DocumentImageSource.FromImage(image));
            _richServer.Document.Paragraphs.Append();

            AddText(new Text("", new SettingText()));
        }

        public void AddTable(TableModel table)
        {
            // Логика формирования/заполнения таблицы

            AddText(new Text("", new SettingText()));

            Table tablePdf = _richServer.Document.Tables.Create(_richServer.Document.Selection.Start, table.TableData.Count, table.HeaderTable.Headers.Count, AutoFitBehaviorType.AutoFitToContents);

            DocumentRange range = _richServer.Document.Tables[_richServer.Document.Tables.Count - 1].Range;
            CharacterProperties titleFormatting = _richServer.Document.BeginUpdateCharacters(range);
       
            titleFormatting.FontSize = 14.0f;
            titleFormatting.FontName = "Helvetica";
            titleFormatting.ForeColor = Color.Red;

            _richServer.Document.EndUpdateCharacters(titleFormatting);

            tablePdf.Borders.InsideHorizontalBorder.LineThickness = 0;
            tablePdf.Borders.InsideHorizontalBorder.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.InsideVerticalBorder.LineThickness = 0;
            tablePdf.Borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.None;

            tablePdf.TableAlignment = TableRowAlignment.Center;
            tablePdf.HorizontalAlignment = TableHorizontalAlignment.Center;
            tablePdf.VerticalAlignment = TableVerticalAlignment.Center;

            tablePdf.Borders.Left.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.Left.LineThickness = 0;

            tablePdf.Borders.Right.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.Right.LineThickness = 0;

            tablePdf.Borders.Top.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.Top.LineThickness = 0;

            tablePdf.Borders.Bottom.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.Bottom.LineThickness = 0;

            #region Заполнение таблицы

            tablePdf.BeginUpdate();

            table.HeaderTable.Headers.ForEach((headreName, i) =>
            {
                _richServer.Document.InsertText(tablePdf[0, i].Range.Start, headreName);
            });

            for (int i = 0; i < table.TableData.Count - 1; i++)
            {
                for (int j = 0; j < table.HeaderTable.Headers.Count; j++)
                {
                    _richServer.Document.InsertText(tablePdf[i + 1, j].Range.Start, table.TableData[i][j].Text.Letter);
                }
            }

            tablePdf.EndUpdate();

            _richServer.Document.Paragraphs.Append();

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

            _richServer.Document.AppendText(text.Letter);
            _richServer.Document.Paragraphs.Append();

            _richServer.Document.EndUpdateCharacters(titleFormatting);
        }

        public void OpenPreview()
        {
            _link.Component = _richServer;

            _link.CreateDocument();
            _link.ShowPreviewDialog();
        }

        public void SaveDocument()
        {
            using (FileStream fs = new FileStream($@"../../{Guid.NewGuid()}.pdf", FileMode.OpenOrCreate))
            {
                _link.Component = _richServer;
                _link.CreateDocument();

                _link.ExportToPdf(fs);
            }
        }

        /// <summary>
        /// Вызов методов в нужной последовательности (который заложит пользователь при добавлении в список)
        /// </summary>
        /// <param name="actions"></param>
        public void GetCallSequenceMethods(IEnumerable<Action> actions)
        {
            foreach (var action in actions)
            {
                action();
            }
        }
    }
}
