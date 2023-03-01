using Export.Interfaces;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using Export.Models;
using DevExpress.XtraPrinting;
using System.Drawing;
using Export.Enums;

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

            BorderLineStyle borderLineStyle = table.TableSetting.TableBorderInsideSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Dotted)
                ? BorderLineStyle.Dotted : table.TableSetting.TableBorderInsideSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Double)
                ? BorderLineStyle.Double : table.TableSetting.TableBorderInsideSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Thick)
                ? BorderLineStyle.Thick : table.TableSetting.TableBorderInsideSetting.BorderLineStyle.Equals(SettingBorderLineStyle.DotDash)
                ? BorderLineStyle.DashDot : table.TableSetting.TableBorderInsideSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Dashed)
                ? BorderLineStyle.Dashed : table.TableSetting.TableBorderInsideSetting.BorderLineStyle.Equals(SettingBorderLineStyle.DotDotDash)
                ? BorderLineStyle.DashDotDot : table.TableSetting.TableBorderInsideSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Single)
                ? BorderLineStyle.Thin : BorderLineStyle.None;

            BorderLineStyle borderLineStyle2 = table.TableSetting.TableBorderSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Dotted)
               ? BorderLineStyle.Dotted : table.TableSetting.TableBorderSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Double)
               ? BorderLineStyle.Double : table.TableSetting.TableBorderSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Thick)
               ? BorderLineStyle.Thick : table.TableSetting.TableBorderSetting.BorderLineStyle.Equals(SettingBorderLineStyle.DotDash)
               ? BorderLineStyle.DashDot : table.TableSetting.TableBorderSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Dashed)
               ? BorderLineStyle.Dashed : table.TableSetting.TableBorderSetting.BorderLineStyle.Equals(SettingBorderLineStyle.DotDotDash)
               ? BorderLineStyle.DashDotDot : table.TableSetting.TableBorderSetting.BorderLineStyle.Equals(SettingBorderLineStyle.Single)
               ? BorderLineStyle.Thin : BorderLineStyle.None;

            SpreadsheetHorizontalAlignment spreadsheetHeaderHorizontal = table.TableSetting.HeaderSetting.SettingText.TextAligment.Equals(Aligment.Center)
                ? SpreadsheetHorizontalAlignment.Center : table.TableSetting.HeaderSetting.SettingText.TextAligment.Equals(Aligment.Left)
                ? SpreadsheetHorizontalAlignment.Left : table.TableSetting.HeaderSetting.SettingText.TextAligment.Equals(Aligment.Right)
                ? SpreadsheetHorizontalAlignment.Right : SpreadsheetHorizontalAlignment.Justify;

            SpreadsheetHorizontalAlignment spreadsheetBodyHorizontal = table.TableSetting.BodySetting.SettingText.TextAligment.Equals(Aligment.Center)
               ? SpreadsheetHorizontalAlignment.Center : table.TableSetting.BodySetting.SettingText.TextAligment.Equals(Aligment.Left)
               ? SpreadsheetHorizontalAlignment.Left : table.TableSetting.BodySetting.SettingText.TextAligment.Equals(Aligment.Right)
               ? SpreadsheetHorizontalAlignment.Right : SpreadsheetHorizontalAlignment.Justify;

            #region Заполнение заголовков таблицы

            for (int i = 0; i < table.HeaderTable.Headers.Count; i++)
            {
                _worksheet.Cells[rowIndex, i].SetValue(table.HeaderTable.Headers[i]);

                #region Настройка ячеек заголовков

                _worksheet.Cells[rowIndex, i].Alignment.Horizontal = spreadsheetHeaderHorizontal;
                _worksheet.Cells[rowIndex, i].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                _worksheet.Cells[rowIndex, i].Fill.BackgroundColor = table.TableSetting.HeaderSetting.BackGroundColor;
                _worksheet.Cells[rowIndex, i].Font.Color = table.TableSetting.HeaderSetting.SettingText.Color;
                _worksheet.Cells[rowIndex, i].Font.Bold = table.TableSetting.HeaderSetting.SettingText.Bold;
                _worksheet.Cells[rowIndex, i].Font.Italic = table.TableSetting.HeaderSetting.SettingText.Italic;
                _worksheet.Cells[rowIndex, i].Font.Size = Convert.ToDouble(table.TableSetting.HeaderSetting.SettingText.FontSize);

                if (!string.IsNullOrEmpty(table.TableSetting.HeaderSetting.SettingText.FontName))
                    _worksheet.Cells[rowIndex, i].Font.Name = table.TableSetting.HeaderSetting.SettingText.FontName;

                _worksheet.Cells[rowIndex, i].Borders.TopBorder.LineStyle = borderLineStyle2;
                _worksheet.Cells[rowIndex, i].Borders.LeftBorder.LineStyle = borderLineStyle2;
                _worksheet.Cells[rowIndex, i].Borders.RightBorder.LineStyle = borderLineStyle2;
                _worksheet.Cells[rowIndex, i].Borders.BottomBorder.LineStyle = borderLineStyle2;

                _worksheet.Cells[rowIndex, i].Borders.InsideHorizontalBorders.LineStyle = borderLineStyle;
                _worksheet.Cells[rowIndex, i].Borders.InsideVerticalBorders.LineStyle = borderLineStyle;

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

                    _worksheet.Cells[rowIndex + i + 1, j].Alignment.Horizontal = spreadsheetBodyHorizontal;
                    _worksheet.Cells[rowIndex + i + 1, j].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                    if (!string.IsNullOrEmpty(table.TableSetting.BodySetting.SettingText.FontName))
                        _worksheet.Cells[rowIndex + i + 1, j].Font.Name = table.TableSetting.BodySetting.SettingText.FontName;

                    _worksheet.Cells[rowIndex + i + 1, j].Font.Bold = table.TableSetting.BodySetting.SettingText.Bold;
                    _worksheet.Cells[rowIndex + i + 1, j].Font.Italic = table.TableSetting.BodySetting.SettingText.Italic;
                    _worksheet.Cells[rowIndex + i + 1, j].Font.Size = Convert.ToDouble(table.TableSetting.BodySetting.SettingText.FontSize);

                    _worksheet.Cells[rowIndex + i + 1, j].Borders.TopBorder.LineStyle = borderLineStyle2;
                    _worksheet.Cells[rowIndex + i + 1, j].Borders.RightBorder.LineStyle = borderLineStyle2;
                    _worksheet.Cells[rowIndex + i + 1, j].Borders.BottomBorder.LineStyle = borderLineStyle2;
                    _worksheet.Cells[rowIndex + i + 1, j].Borders.LeftBorder.LineStyle = borderLineStyle2;

                    _worksheet.Cells[rowIndex + i + 1, j].Borders.InsideVerticalBorders.LineStyle = borderLineStyle;
                    _worksheet.Cells[rowIndex + i + 1, j].Borders.InsideHorizontalBorders.LineStyle = borderLineStyle;


                    if (table.TableSetting.BodySetting.ColorRow.ColorEveryRow > 0
                        && (i + 1) % table.TableSetting.BodySetting.ColorRow.ColorEveryRow == 0)
                    {
                        _worksheet.Cells[rowIndex + i + 1, j].Fill.BackgroundColor = table.TableSetting.BodySetting.ColorRow.BackGroundColor;
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
