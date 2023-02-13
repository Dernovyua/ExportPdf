using Export.Interfaces;
using System.Drawing;

namespace Export.Models.Charts
{
    public class Line : IChart
    {
        /// <summary>
        /// Список линий, которые необходимо построить
        /// </summary>
       // public IEnumerable<LineData> Lines { get; set; } = Array.Empty<LineData>();

        public SettingChart SettingChart { get; set; }

        public Line()
        {
            SettingChart = new SettingChart();
        }

        public Image CreateChartImage()
        {
            return null;
        }
    }
}
