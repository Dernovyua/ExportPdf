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

        public SettingText SettingText { get; set; }
        public MarkerSetting MarkerSetting { get; set; }

        /// <summary>
        /// Ширина графика
        /// </summary>
        public int Width { get; set; } = 600;

        /// <summary>
        /// Высота графика
        /// </summary>
        public int Height { get; set; } = 350;

        /// <summary>
        /// Подпись графика по оси Y
        /// </summary>
        public string SignatureY { get; set; } = string.Empty;

        /// <summary>
        /// Подпись графика по оси X
        /// </summary>
        public string SignatureX { get; set; } = string.Empty;

        public Color ChartColor { get; set; } = Color.Blue;

        public SettingChart()
        {
            SettingText = new SettingText();
            MarkerSetting = new MarkerSetting();
        }
    }

    public class MarkerSetting
    {
        public int MarkerStart { get; set; } = 0;
        public int MarkerCount { get; set; } = 0;

        public Color MarkerColor { get; set; } = Color.Red;
    }
}
