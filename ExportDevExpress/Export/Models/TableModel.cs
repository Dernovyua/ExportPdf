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

        public TableSetting TableSetting { get; set; }

        public List<List<object>> TableData { get; set; } = new();

        public TableModel(HeaderTable headerTable, TableSetting tableSetting, List<List<object>> tableData)
        {
            HeaderTable = headerTable;
            TableData = tableData;
            TableSetting = tableSetting;
        }

        public TableModel()
        {
            TableSetting = new TableSetting();
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
                ParagraphAlignment = ParagraphAlignment.Center,
            };
        }
    }

    public class TableBorderSetting
    {
        public BorderLineStyle BorderLineStyle { get; set; }
        public float LineThickness { get; set; } = 0.0f;
    }

    public class TableBorderInsideSetting : TableBorderSetting
    {

    }

    public class TableAligment
    {
        public ParagraphAlignment ParagraphAlignment { get; set; } = ParagraphAlignment.Center;
    }
}
