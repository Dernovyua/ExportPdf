using Export.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace Export.Models
{
    /// <summary>
    /// Модель таблицы, состоящая из настроек/заголовков/данных таблицы
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
        /// Настройки заголовков
        /// </summary>
        public HeaderSetting HeaderSetting { get; set; }

        /// <summary>
        /// Настройки таблицы
        /// </summary>
        public BodySetting BodySetting { get; set; }

        /// Настройка внешних границ таблицы
        /// </summary>
        public TableBorderSetting TableBorderSetting { get; set; }

        /// <summary>
        /// Настройка внутренних границ таблицы
        /// </summary>
        public TableBorderInsideSetting TableBorderInsideSetting { get; set; }

        /// <summary>
        /// Настройки по умолчанию
        /// </summary>
        public TableSetting()
        {
            HeaderSetting = new HeaderSetting();
            BodySetting = new BodySetting();
            TableBorderSetting = new TableBorderSetting() { BorderLineStyle = SettingBorderLineStyle.None };
            TableBorderInsideSetting = new TableBorderInsideSetting() { BorderLineStyle = SettingBorderLineStyle.None };
        }
    }

    public class TableBorderSetting
    {
        public SettingBorderLineStyle BorderLineStyle { get; set; }
        public float LineThickness { get; set; } = 1.0f;
    }

    public class TableBorderInsideSetting : TableBorderSetting
    {

    }

    public class HeaderSetting
    {
        public SettingText SettingText { get; set; }
        public Color BackGroundColor { get; set; }

        /// <summary>
        /// На каждой новой странице вначале таблицы будут отображаться названия заголовков
        /// </summary>
        public bool RepeatHeaderEveryPage { get; set; }

        /// <summary>
        /// Создаются настройки по умолчанию
        /// </summary>
        public HeaderSetting()
        {
            SettingText = new SettingText() { Bold = true, Color = Color.White, TextAligment = Aligment.Center };

            BackGroundColor = Color.FromArgb(64, 66, 166);
            RepeatHeaderEveryPage = true;
        }
    }

    public class BodySetting
    {
        public SettingText SettingText { get; set; }
        public ColorRow ColorRow { get; set; }

        public BodySetting()
        {
            SettingText = new SettingText() { Bold = false, Color = Color.Black, TextAligment = Aligment.Center };
            ColorRow = new ColorRow() { BackGroundColor = Color.FromArgb(220, 230, 242), ColorEveryRow = 2 };
        }
    }

    public class ColorRow
    {
        /// <summary>
        /// Подкрашивание каждой n строки в таблице (По умолчанию, каждая вторая строка)
        /// </summary>
        public ushort ColorEveryRow { get; set; }

        public Color BackGroundColor { get; set; }
    }
}
