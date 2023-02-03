using Export.Interfaces;
using System.Drawing;

namespace Export.Models
{
    /// <summary>
    /// График
    /// </summary>
    public class Chart
    {
        /// <summary>
        /// Интерфейс
        /// </summary>
        private IChart _chart { get; set; }

        public Chart(IChart chart)
        {
            _chart = chart;
        }

        /// <summary>
        /// Создание изображения
        /// </summary>
        /// <returns></returns>
        public Image CreateImage()
        {
            return _chart.CreateImageFromControl();
        }
    }
}
