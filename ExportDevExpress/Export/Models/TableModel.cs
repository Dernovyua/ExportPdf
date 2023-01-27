using Export.Enums;
using System.Collections.Generic;

namespace Export.Models
{
    /// <summary>
    /// Модель таблицы, состощая из настроек/заголовков/данных таблицы
    /// </summary>
    public class TableModel
    {
        public HeaderTable HeaderTable { get; set; } = default!;

        public TableSetting TableSetting { get; set; } = default!;

        public List<List<string>> TableData { get; set; } = new();

        public TableModel(HeaderTable headerTable, TableSetting tableSetting, List<List<string>> tableData)
        {
            HeaderTable = headerTable;
            TableData = tableData;
            TableSetting = tableSetting;
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
    /// Настройки таблицы
    /// </summary>
    public class TableSetting
    {
        /// <summary>
        /// Настройки текста (шрифт, размер, цвет и т.п)
        /// </summary>
        public SettingText SettingText { get; set; }

        /// <summary>
        /// Настройка внешних границ таблицы
        /// </summary>
        public TableBorderSetting TableBorderSetting { get; set; }

        /// <summary>
        /// Настройка внутренних границ таблицы
        /// </summary>
        public TableBorderInsideSetting TableBorderInsideSetting { get; set; } 

        /// <summary>
        /// Настройка выравнивая элементов в таблице
        /// </summary>
        public TableAligment TableAligment { get; set; }

        /// <summary>
        /// На каждой новой странице вначале таблицы будут отображаться названия заголовков
        /// </summary>
        public bool RepeatHeaderEveryPage { get; set; } = true;

        /// <summary>
        /// Настройки по умолчанию
        /// </summary>
        public TableSetting()
        {
            SettingText = new SettingText();
            TableBorderSetting = new TableBorderSetting()
            {
                BorderLineStyle = BorderLineStyle.Single,
                LineThickness = 1.0f
            };
            TableBorderInsideSetting = new TableBorderInsideSetting()
            {
                BorderLineStyle= BorderLineStyle.Single,
                LineThickness = 1.0f
            };

            TableAligment = new TableAligment()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                RowAlignment = RowAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
        }
    }

    public class TableBorderSetting
    {
        public BorderLineStyle BorderLineStyle { get; set; } = BorderLineStyle.Single;
        public float LineThickness { get; set; } = 0.0f;
    }

    public class TableBorderInsideSetting : TableBorderSetting
    {

    }

    public class TableAligment
    {
        public RowAlignment RowAlignment { get; set; } = RowAlignment.Center;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
    }
}
