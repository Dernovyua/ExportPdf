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
        public string Name { get; set; }

        /// <summary>
        /// Ширина графика
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота графика
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Подпись графика по оси Y
        /// </summary>
        public string SignatureY { get; set; } = string.Empty;

        /// <summary>
        /// Подпись графика по оси X
        /// </summary>
        public string SignatureX { get; set; } = string.Empty;

        public SettingChart(string name, int width = 600, int height = 350, string signatureY = "", string signatureX = "")
        {
            Name = name;

            Width = width;
            Height = height;

            SignatureY = signatureY;
            SignatureX = signatureX;
        }

        public SettingChart(string name)
        {
            Name = name;
        }
    }
}
