using Export.Interfaces;
using System.Drawing;

namespace Export.Models.Charts
{
    public class Pie : IChart
    {
        public PieData PieData { get; set; }

        public Pie()
        {
            
        }

        public Image CreateImageFromControl()
        {
            return null;
        }
    }

    public class PieData
    {

    }
}
