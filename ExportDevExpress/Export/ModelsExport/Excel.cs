using Export.Interfaces;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using Export.Models;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace Export.ModelsExport
{
    public class Excel : IReport
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

        private string _path { get; set; }
        private string _nameFile { get; set; }

        private int rowIndex { get; set; } = 0;
        private int columnIndex { get; set; } = 0;

        public Excel(string path, string nameFile, string? nameSheet = null)
        {
            _workbook = new Workbook();
            _worksheet = _workbook.Worksheets[^1];

            if (!string.IsNullOrEmpty(nameSheet))
                _workbook.Worksheets[0].Name = nameSheet;

            _printingSystem = new PrintingSystem();
            _link = new PrintableComponentLink(_printingSystem);

            _path = path;
            _nameFile = nameFile;
        }

        public void AddChart(Chart chart)
        {
            Image image = chart.GetImage();

            _workbook.BeginUpdate();
            _worksheet = _workbook.Worksheets[^1];

            _worksheet.Pictures.AddPicture(image, _worksheet[rowIndex + 1, 0]);

            rowIndex = _worksheet.Pictures[^1].BottomRightCell.BottomRowIndex + 2;
            columnIndex = _worksheet.Pictures[^1].TopLeftCell.LeftColumnIndex;

            _workbook.EndUpdate();
        }

        public void AddTable(TableModel table)
        {
            _workbook.BeginUpdate();
            _worksheet = _workbook.Worksheets[^1];

            #region Заполнение заголовков таблицы

            for (int i = 0; i < table.HeaderTable.Headers.Count; i++)
            {
                _worksheet.Cells[rowIndex, i].SetValue(table.HeaderTable.Headers[i]);

                #region Настройка ячеек заголовков

                _worksheet.Cells[rowIndex, i].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                _worksheet.Cells[rowIndex, i].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                _worksheet.Cells[rowIndex, i].Fill.BackgroundColor = Color.FromArgb(64, 66, 166);
                _worksheet.Cells[rowIndex, i].Font.Color = Color.White;
                _worksheet.Cells[rowIndex, i].Font.Bold = true;

                #endregion
            }

            #endregion

            #region Заполнение таблицы

            for (int i = 0; i < table.TableData.Count; i++)
            {
                for (int j = 0; j < table.HeaderTable.Headers.Count; j++)
                {
                    _worksheet.Cells[rowIndex + i + 1, j].SetValue(table.TableData[i][j]);

                    #region Настройка ячеек

                    _worksheet.Cells[rowIndex + i + 1, j].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    _worksheet.Cells[rowIndex + i + 1, j].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                    if ((i + 1) % 2 == 0)
                    {
                        _worksheet.Cells[rowIndex + i + 1, j].Fill.BackgroundColor = Color.FromArgb(220, 230, 242);
                    }

                    #endregion
                }
            }

            #endregion

            columnIndex = table.HeaderTable.Headers.Count;
            rowIndex += table.TableData.Count + 2;

            _worksheet.Rows.AutoFit(0, table.TableData.Count);
            _worksheet.Columns.AutoFit(0, table.HeaderTable.Headers.Count);

            _workbook.EndUpdate();
        }

        public void AddText(Text text)
        {
            // не используется, т.к не добавляем текст в эксель
        }

        public void AddNewPage(string? name = null)
        {
            rowIndex = 0;
            columnIndex = 0;

            _workbook.Worksheets.Add(name);
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
            _workbook.Worksheets.ActiveWorksheet = _workbook.Worksheets[0];

            _workbook.SaveDocument($@"{_path}/{_nameFile}.xlsx", DocumentFormat.Xlsx);
        }
    }
}
