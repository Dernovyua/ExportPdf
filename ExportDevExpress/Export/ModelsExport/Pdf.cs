using DevExpress.DataAccess.Native.Json;
using DevExpress.Mvvm.Native;
using DevExpress.Printing.Core.PdfExport.Metafile;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Export.Enums;
using Export.Interfaces;
using Export.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        }

        public void AddTable(TableModel table)
        {
            // Логика формирования/заполнения таблицы

            Table tablePdf = _richServer.Document.Tables.Create(_richServer.Document.Selection.Start, table.TableData.Count + 1, table.HeaderTable.Headers.Count, AutoFitBehaviorType.AutoFitToContents);

            #region Вынести в настройки

            tablePdf.Borders.Left.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.Left.LineThickness = 0;

            tablePdf.Borders.Right.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.Right.LineThickness = 0;

            tablePdf.Borders.Top.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.Top.LineThickness = 0;

            tablePdf.Borders.Bottom.LineStyle = TableBorderLineStyle.None;
            tablePdf.Borders.Bottom.LineThickness = 0;

            #endregion

            #region Заполнение таблицы

            tablePdf.BeginUpdate();

            table.HeaderTable.Headers.ForEach((headreName, i) =>
            {
                _richServer.Document.InsertText(tablePdf[0, i].Range.Start, headreName);
            });

            for (int i = 0; i < table.TableData.Count; i++)
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

            Table tableText = _richServer.Document.Tables.Create(_richServer.Document.Selection.Start, 1, 1, AutoFitBehaviorType.AutoFitToContents);

            tableText.BeginUpdate();

            tableText.TableAlignment = text.SettingText.TextAligment.Equals(Aligment.Left) ? TableRowAlignment.Left
                : text.SettingText.TextAligment.Equals(Aligment.Right) ? TableRowAlignment.Right 
                : text.SettingText.TextAligment.Equals(Aligment.Center) ? TableRowAlignment.Center : TableRowAlignment.Both;

            tableText.Style.FontSize = text.SettingText.FontSize;
            tableText.Style.Bold = text.SettingText.Bold;
            tableText.Style.Italic = text.SettingText.Italic;

            tableText.Borders.Left.LineStyle = TableBorderLineStyle.None;
            tableText.Borders.Left.LineThickness = 0;

            tableText.Borders.Right.LineStyle = TableBorderLineStyle.None;
            tableText.Borders.Right.LineThickness = 0;

            tableText.Borders.Top.LineStyle = TableBorderLineStyle.None;
            tableText.Borders.Top.LineThickness = 0;

            tableText.Borders.Bottom.LineStyle = TableBorderLineStyle.None;
            tableText.Borders.Bottom.LineThickness = 0;

            _richServer.Document.InsertText(tableText[0, 0].Range.Start, text.Letter);

            tableText.EndUpdate();

            _richServer.Document.Paragraphs.Append();
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
