using Export.Enums;
using System;
using System.Collections.Generic;

namespace Export.Models
{
    public class Chart
    {
        /// <summary>
        /// Настройки графика
        /// </summary>
        public SettingChart SettingChart { get; set; }

        /// <summary>
        /// Данные графика
        /// </summary>
        public ChartData ChartData { get; set; }

        public Chart(SettingChart settingChart, ChartData chartData)
        {
            SettingChart = settingChart;
            ChartData = chartData;
        }
    }

    /// <summary>
    /// Настройки графика
    /// </summary>
    public class SettingChart
    {
        /// <summary>
        /// Ширина графика
        /// </summary>
        public float? Width { get; set; }

        /// <summary>
        /// Высота графика
        /// </summary>
        public float? Height { get; set; }

        /// <summary>
        /// Подпись графика по оси Y
        /// </summary>
        public string SignatureY { get; set; } = string.Empty;

        /// <summary>
        /// Подпись графика по оси X
        /// </summary>
        public string SignatureX { get; set; } = string.Empty;

        /// <summary>
        /// Тип графика
        /// </summary>
        public TypeChart TypeChart { get; set; }

        public SettingChart(float? width, float? height, string signatureY, string signatureX, TypeChart typeChart)
        {
            Width = width;
            Height = height;
            SignatureY = signatureY;
            SignatureX = signatureX;
            TypeChart = typeChart;
        }
    }

    /// <summary>
    /// Данные графика
    /// </summary>
    public class ChartData
    {
        /// <summary>
        /// Значения
        /// </summary>
        public IEnumerable<double> Data { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Названия
        /// </summary>
        public IEnumerable<string> Name { get; set; } = Array.Empty<string>();
    }
}
