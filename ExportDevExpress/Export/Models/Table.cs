using System;
using System.Collections.Generic;

namespace Export.Models
{
    public class Table
    {
        public HeaderTable HeaderTable { get; set; }
        public IEnumerable<IEnumerable<Cell>> TableData { get; set; } = Array.Empty<IEnumerable<Cell>>();

        public Table(HeaderTable headerTable, IEnumerable<IEnumerable<Cell>> tableData)
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
        public IEnumerable<string> Headers { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Ячейка таблицы
    /// </summary>
    public class Cell
    {
        public string Value { get; set; }
        public Text Text { get; set; }

        public Cell(string value, Text text)
        {
            Value = value;
            Text = text;
        }
    }
}
