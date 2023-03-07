using Export.DrawingCharts;
using Export.Interfaces;
using System.Drawing;

namespace Export.Models.Charts
{
    public class Line : IChart
    {
        public SettingChart SettingChart { get; set; }

        private LineETS _lineETS { get; set; }

        public Line(LineETS lineETS)
        {
            _lineETS = lineETS;
        }

        public Image CreateChartImage()
        {
            _lineETS.Draw();

            return _lineETS.GetImage();
        }
    }
}

