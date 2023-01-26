using DevExpress.XtraCharts;
using Export.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Export.Models.Charts
{
    public class Histrogram : IChart
    {
        public IEnumerable<HistrogramData> HistrogramData { get; set; } = Array.Empty<HistrogramData>();

        public Histrogram(IEnumerable<HistrogramData> histrogramData)
        {
            HistrogramData = histrogramData;
        }

        public Histrogram()
        {
            
        }

        public Image CreateImageFromControl()
        {
            ChartControl chartControl = new ChartControl();

            Series histogram = new Series("Histogram", ViewType.Bar);
            histogram.ArgumentDataMember = "XValue"; // Название свойства, которое отвечает за значение по оси X
            histogram.ValueDataMembers[0] = "YValue";

            chartControl.Height = 250;
            chartControl.Width = 600;

            chartControl.Series.Add(histogram);

            ((BarSeriesView)histogram.View).AggregateFunction = SeriesAggregateFunction.Histogram;

            histogram.DataSource = HistrogramData;

            XYDiagram diagram = (XYDiagram)chartControl.Diagram;
            diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Interval;
            diagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;

            diagram.AxisX.WholeRange.SideMarginsValue = 0;
            diagram.AxisX.NumericScaleOptions.IntervalOptions.Count = 20;
            diagram.AxisX.NumericScaleOptions.IntervalOptions.DivisionMode = IntervalDivisionMode.Auto;

            diagram.AxisX.Label.TextPattern = "{}{OB}{A1:F1}, {A2:F1}{CB}";

            using (MemoryStream s = new MemoryStream())
            {
                chartControl.ExportToImage(s, ImageFormat.Jpeg);
                return Image.FromStream(s);
            }
        }
    }

    public class HistrogramData
    {
        public double XValue { get; set; }
        public double YValue { get; set; }

        public HistrogramData(double xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
    }
}
