using Export.Interfaces;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using Export.Models;
using DevExpress.XtraPrinting;
using System.IO;
using System.Drawing;
using System.Linq;

namespace Export.ModelsExport
{
    public class Csv : ISequence
    {
        private Workbook _workbook { get; set; }
        private Worksheet _worksheet { get; set; }
        private PrintingSystem _printingSystem { get; set; }
        private PrintableComponentLink _link { get; set; }

        public Csv()
        {
            _workbook = new Workbook();

            _printingSystem = new PrintingSystem();
            _link = new PrintableComponentLink(_printingSystem);
        }

        public void AddChart(Chart chart)
        {
            //Image image = chart.CreateImage();

            //_worksheet = _workbook.Worksheets[0];
            //_workbook.BeginUpdate();

            //_worksheet.Pictures.AddPicture(@"C:\Users\Flax\Desktop\photo_2023-01-27_01-10-57.jpg", _worksheet[23, 0]);

            //_workbook.EndUpdate();
        }

        public void AddTable(TableModel table)
        {
            CustomTableStyle(_workbook, table);

            _workbook.SaveDocument($@"../../{Guid.NewGuid()}.xlsx", DocumentFormat.Xlsx);
        }

        public void CreateTable(IWorkbook workbook, TableModel tableModel)
        {
            _worksheet = workbook.Worksheets[0];
            _workbook.BeginUpdate();

            Table table = _worksheet.Tables.Add(_worksheet[0, 0], true);

            // Format the table by applying a built-in table style.
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

            // Specify horizontal alignment for header and total rows of the table.
            table.HeaderRowRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            table.HeaderRowRange.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

            table.Range.ColumnWidthInCharacters = 20;

            InsertTable(_worksheet, tableModel);

            workbook.EndUpdate();

            //_worksheet.Pictures.AddPicture(@"C:\Users\Flax\Desktop\photo_2023-01-27_01-10-57.jpg", _worksheet[23, 0]);
        }

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

        public void CustomTableStyle(IWorkbook workbook, TableModel tableModel)
        {
            CreateTable(workbook, tableModel);

            workbook.BeginUpdate();

            Worksheet worksheet = workbook.Worksheets[0];

            #region #CustomTableStyle
            // Access a table.
            Table table = worksheet.Tables[0];

            string styleName = "testTableStyle";

            // If the style under the specified name already exists in the collection,
            if (workbook.TableStyles.Contains(styleName))
            {
                // apply this style to the table.
                table.Style = workbook.TableStyles[styleName];
            }
            else
            {
                // Add a new table style under the "testTableStyle" name to the TableStyles collection.
                TableStyle customTableStyle = workbook.TableStyles.Add("testTableStyle");

                // Modify the required formatting characteristics of the table style. 
                // Specify the format for different table elements.
                customTableStyle.BeginUpdate();
                try
                {
                    customTableStyle.TableStyleElements[TableStyleElementType.WholeTable].Font.Color = Color.FromArgb(107, 107, 107);

                    // Specify formatting characteristics for the table header row. 
                    TableStyleElement headerRowStyle = customTableStyle.TableStyleElements[TableStyleElementType.HeaderRow];
                    headerRowStyle.Fill.BackgroundColor = Color.FromArgb(64, 66, 166);
                    headerRowStyle.Font.Color = Color.White;
                    headerRowStyle.Font.Bold = true;

                    // Specify formatting characteristics for the table total row. 
                    TableStyleElement totalRowStyle = customTableStyle.TableStyleElements[TableStyleElementType.TotalRow];
                    totalRowStyle.Fill.BackgroundColor = Color.FromArgb(115, 193, 211);
                    totalRowStyle.Font.Color = Color.White;
                    totalRowStyle.Font.Bold = true;

                    // Specify banded row formatting for the table.
                    TableStyleElement secondRowStripeStyle = customTableStyle.TableStyleElements[TableStyleElementType.SecondRowStripe];
                    //secondRowStripeStyle.Fill.BackgroundColor = Color.FromArgb(234, 234, 234);
                    secondRowStripeStyle.Fill.BackgroundColor = Color.FromArgb(220, 230, 242);
                    secondRowStripeStyle.StripeSize = 1;
                }
                finally
                {
                    customTableStyle.EndUpdate();
                }
                // Apply the created custom style to the table.
                table.Style = customTableStyle;
            }
            #endregion #CustomTableStyle

            workbook.EndUpdate();
        }

        public void AddText(Text text)
        {

        }

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
            using (FileStream fs = new FileStream($@"../../{Guid.NewGuid()}.xlsx", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                _link.Component = _workbook;

                _link.CreateDocument();
                _link.ExportToXlsx(fs);
            }
        }

        public void AddNewPage()
        {
            throw new NotImplementedException();
        }
    }
}
