﻿using Export.Enums;
using System.Drawing;

namespace Export.Models
{
    /// <summary>
    /// Настройки графика
    /// </summary>
    public class SettingChart
    {
        /// <summary>
        /// Название графика
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Измерение графика (2D или 3D)
        /// </summary>
        public Dimension Dimension { get; set; } = Dimension.Two;

        public SettingText SettingText { get; set; }

        /// <summary>
        /// Ширина графика
        /// </summary>
        public int Width { get; set; } = 600;

        /// <summary>
        /// Высота графика
        /// </summary>
        public int Height { get; set; } = (int)(600.0 * (297.0 / 210.0) / 1.2);

        /// <summary>
        /// Цвет столбцов гистограммы
        /// </summary>
        public Color Color { get; set; } =  Color.Blue;

        /// <summary>
        /// Подпись графика по оси Y
        /// </summary>
        public string SignatureY { get; set; } = string.Empty;

        /// <summary>
        /// Подпись графика по оси X
        /// </summary>
        public string SignatureX { get; set; } = string.Empty;

        public SettingChart()
        {
            SettingText = new SettingText();
        }
    }
}
