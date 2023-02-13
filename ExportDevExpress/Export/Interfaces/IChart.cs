using System.Drawing;

namespace Export.Interfaces
{
    public interface IChart
    {
        /// <summary>
        /// Создание изображения по графическому контролу
        /// </summary>
        /// <returns></returns>
        Image CreateChartImage();
    }
}
