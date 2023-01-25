﻿using Export.Enums;
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
    }

    /// <summary>
    /// Настройки текста
    /// </summary>
    public class SettingText
    {
        public Color Color { get; set; } = Color.Black;
        public bool? Bold { get; set; } = false;
        public bool? Italic { get; set; } = false;
        public float? FontSize { get; set; } = 14.0f;
        public TextAligment TextAligment { get; set; } = TextAligment.Justify;
    }
}
