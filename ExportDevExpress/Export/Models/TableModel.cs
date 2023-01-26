using System;
using System.Collections.Generic;

namespace Export.Models
{
    public class TableModel
    {
        public HeaderTable HeaderTable { get; set; }
        public List<List<Cell>> TableData { get; set; } = new();

        public TableModel(HeaderTable headerTable, List<List<Cell>> tableData)
        {
            HeaderTable = headerTable;
            TableData = tableData;
        }
    }

    /// <summary>
    /// Названия колонок таблицы
    /// </summary>
    public class HeaderTable
    {
        public List<string> Headers { get; set; } = new();
    }

    /// <summary>
    /// Ячейка таблицы
    /// </summary>
    public class Cell
    {
        public Text Text { get; set; }

        public Cell(Text text)
        {
            Text = text;
        }
    }
}
