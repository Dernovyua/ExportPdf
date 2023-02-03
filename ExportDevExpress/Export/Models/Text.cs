using Export.Enums;
using System.Drawing;

namespace Export.Models
{
    /// <summary>
    /// Текст
    /// </summary>
    public class Text
    {
        /// <summary>
        /// Текст
        /// </summary>
        public string Letter { get; set; } = string.Empty;

        /// <summary>
        /// Настройка текста
        /// </summary>
        public SettingText SettingText { get; set; }

        public Text(string letter, SettingText settingText)
        {
            Letter = letter;
            SettingText = settingText;
        }

        public Text(string letter)
        {
            Letter = letter;
            SettingText = new SettingText()
            {
                Bold = false,
                Color = Color.Black,
                FontName = "Helvetica",
                FontSize = 14.0f,
                Italic = false,
                TextAligment = Aligment.Justify
            };
        }
    }

    /// <summary>
    /// Настройки текста
    /// </summary>
    public class SettingText
    {
        /// <summary>
        /// Цвет шрифта
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// Жирный шрифт
        /// </summary>
        public bool? Bold { get; set; } = false;

        /// <summary>
        /// Наклонный шрифт
        /// </summary>
        public bool? Italic { get; set; } = false;

        /// <summary>
        /// Название шрифта
        /// </summary>
        public string FontName { get; set; } = "Helvetica";

        /// <summary>
        /// Размер шрифта
        /// </summary>
        public float? FontSize { get; set; } = 14.0f;

        /// <summary>
        /// Выравнивание текста
        /// </summary>
        public Aligment TextAligment { get; set; } = Aligment.Justify;

        public SettingText()
        {
            
        }
    }
}
