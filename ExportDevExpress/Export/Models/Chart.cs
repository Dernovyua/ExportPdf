using Export.Interfaces;
using System.Drawing;

namespace Export.Models
{
    public class Chart
    {
        private IChart _chart { get; set; }
        public SettingChart SettingChart { get; set; }

        public Chart(IChart chart, SettingChart settingChart)
        {
            _chart = chart;
            SettingChart = settingChart;
        }

        public Image CreateImage()
        {
            return _chart.CreateImageFromControl();
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

        public SettingChart(float? width, float? height, string signatureY = "", string signatureX = "")
        {
            Width = width;
            Height = height;

            SignatureY = signatureY;
            SignatureX = signatureX;
        }

        public SettingChart()
        {
            
        }
    }
}
