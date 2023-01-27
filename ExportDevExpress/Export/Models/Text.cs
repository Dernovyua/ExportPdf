using Export.Enums;
using System.Drawing;

namespace Export.Models
{
    public class Text
    {
        /// <summary>
        /// Текст
        /// </summary>
        public string Letter { get; set; } = string.Empty;

        /// <summary>
        /// Настройки текста
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
        public Color Color { get; set; } = Color.Black;
        public bool? Bold { get; set; } = false;
        public bool? Italic { get; set; } = false;
        public string FontName { get; set; } = "Helvetica";
        public float? FontSize { get; set; } = 14.0f;
        public Aligment TextAligment { get; set; } = Aligment.Justify;

        public SettingText()
        {
            
        }
    }
}
