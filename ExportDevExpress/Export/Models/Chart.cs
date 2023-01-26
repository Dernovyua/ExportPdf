using Export.Interfaces;
using System.Drawing;

namespace Export.Models
{
    public class Chart
    {
        private IChart _chart { get; set; }

        public Chart(IChart chart)
        {
            _chart = chart;
        }

        public Image CreateImage()
        {
            return _chart.CreateImageFromControl();
        }
    }
}
