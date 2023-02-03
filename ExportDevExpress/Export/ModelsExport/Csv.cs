using Export.Interfaces;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using Export.Models;
using DevExpress.XtraPrinting;
using System.Drawing;
using System.Linq;

namespace Export.ModelsExport
{
    public class Csv : ISequence
    {
        /// <summary>
        /// Класс для работы с таблицей эксель
        /// </summary>
        private Workbook _workbook { get; set; }

        /// <summary>
        /// Таблица страницы экселя
        /// </summary>
        private Worksheet _worksheet { get; set; }

        #region Необходимо для превью и создания документа

        private PrintingSystem _printingSystem { get; set; }
        private PrintableComponentLink _link { get; set; }

        #endregion

        public Csv()
        {
            _workbook = new Workbook();

            _printingSystem = new PrintingSystem();
            _link = new PrintableComponentLink(_printingSystem);
        }

        public void AddChart(Chart chart)
        {
           // не используется, т.к не добавляем графику в эксель
        }

        public void AddTable(TableModel table)
        {
            CustomTableStyle(_workbook, table);
            AddNewPage();
        }

        public void AddText(Text text)
        {
            // не используется, т.к не добавляем текст в эксель
        }

        public void AddNewPage()
        {
            _workbook.Worksheets.Add();
        }

        private void CreateTable(IWorkbook workbook, TableModel tableModel)
        {
            _worksheet = workbook.Worksheets[^1];
            _workbook.BeginUpdate();

            Table table = _worksheet.Tables.Add(_worksheet[0, 0], true);

            table.Style = workbook.TableStyles[BuiltInTableStyleId.TableStyleDark10];

            for (int i = 0; i < tableModel.HeaderTable.Headers.Count; i++)
            {
                table.Columns.Add();
                table.Columns[i].Name = tableModel.HeaderTable.Headers[i];
                table.Columns[i].DataRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                table.Columns[i].DataRange.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                table.Columns[i].DataRange.NumberFormat = "#,##0.00";
            }

            table.Columns.Last().Delete();

            for (int i = 0; i < tableModel.TableData.Count - 1; i++)
            {
                table.Rows.Add();
            }

            table.HeaderRowRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            table.HeaderRowRange.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

            table.Range.ColumnWidthInCharacters = 20;

            InsertTable(_worksheet, tableModel);

            workbook.EndUpdate();
        }

        /// <summary>
        /// Заполнение таблицы данными
        /// </summary>
        /// <param name="sheet">Страница</param>
        /// <param name="tableModel">данные по которым идет заполнение</param>
        static void InsertTable(Worksheet sheet, TableModel tableModel)
        {
            for (int i = 0; i < tableModel.TableData.Count; i++)
            {
                for (int j = 0; j < tableModel.HeaderTable.Headers.Count; j++)
                {
                    sheet.Cells[i + 1, j].SetValue(tableModel.TableData[i][j]);
                }
            }
        }

        /// <summary>
        /// Создание таблицы с настраиваемым стилем таблицы
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="tableModel"></param>
        public void CustomTableStyle(IWorkbook workbook, TableModel tableModel)
        {
            CreateTable(workbook, tableModel);

            workbook.BeginUpdate();

            Worksheet worksheet = workbook.Worksheets[^1];

            #region Создание стиля таблицы

            Table table = worksheet.Tables[0];

            string styleName = "testTableStyle";

            if (workbook.TableStyles.Contains(styleName))
            {
                table.Style = workbook.TableStyles[styleName];
            }
            else
            {
                TableStyle customTableStyle = workbook.TableStyles.Add("testTableStyle");

                customTableStyle.BeginUpdate();
                try
                {
                    customTableStyle.TableStyleElements[TableStyleElementType.WholeTable].Font.Color = Color.FromArgb(107, 107, 107);

                    TableStyleElement headerRowStyle = customTableStyle.TableStyleElements[TableStyleElementType.HeaderRow];
                    headerRowStyle.Fill.BackgroundColor = Color.FromArgb(64, 66, 166);
                    headerRowStyle.Font.Color = Color.White;
                    headerRowStyle.Font.Bold = true;

                    TableStyleElement totalRowStyle = customTableStyle.TableStyleElements[TableStyleElementType.TotalRow];
                    totalRowStyle.Fill.BackgroundColor = Color.FromArgb(115, 193, 211);
                    totalRowStyle.Font.Color = Color.White;
                    totalRowStyle.Font.Bold = true;

                    TableStyleElement secondRowStripeStyle = customTableStyle.TableStyleElements[TableStyleElementType.SecondRowStripe];
                    secondRowStripeStyle.Fill.BackgroundColor = Color.FromArgb(220, 230, 242);
                    secondRowStripeStyle.StripeSize = 1;
                }
                finally
                {
                    customTableStyle.EndUpdate();
                }
                table.Style = customTableStyle;
            }

            #endregion #CustomTableStyle

            workbook.EndUpdate();
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

        public void OpenPreview()
        {
            _link.Component = _workbook;
            _link.CreateDocument();
            _link.ShowPreviewDialog();
        }

        public void SaveDocument()
        {
            _workbook.SaveDocument($@"../../{Guid.NewGuid()}.xlsx", DocumentFormat.Xlsx);
        }

        
    }
}
