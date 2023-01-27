using DevExpress.XtraCharts;
using Export.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Export.Models.Charts
{
    public class Line : IChart
    {
        /// <summary>
        /// Список линий, которые необходимо построить
        /// </summary>
        public IEnumerable<LineData> Lines { get; set; } = Array.Empty<LineData>();

        public Line()
        {

        }

        public Image CreateImageFromControl()
        {
            ChartControl lineChart = new ChartControl();

            List<Series> series = new List<Series>();

            foreach (LineData line in Lines)
            {
                series.Add(new Series(line.NameLine, ViewType.Line));

                foreach (var item in line.LinePoints)
                {
                    series[series.Count - 1].Points.Add(new SeriesPoint(item.XValue, item.YValue));
                }

                series[series.Count - 1].ArgumentScaleType = ScaleType.Numerical;

                ((LineSeriesView)series[series.Count - 1].View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                ((LineSeriesView)series[series.Count - 1].View).LineMarkerOptions.Size = 20;
                ((LineSeriesView)series[series.Count - 1].View).LineMarkerOptions.Kind = MarkerKind.Circle;
                ((LineSeriesView)series[series.Count - 1].View).LineStyle.DashStyle = DashStyle.Dash;
            }

            lineChart.Series.AddRange(series.ToArray());

            ((XYDiagram)lineChart.Diagram).AxisY.Interlaced = true;
            ((XYDiagram)lineChart.Diagram).AxisY.InterlacedColor = Color.FromArgb(20, 60, 60, 60);
            ((XYDiagram)lineChart.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
            ((XYDiagram)lineChart.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;

            lineChart.Width = 600;
            lineChart.Height = 350;

            lineChart.Titles.Add(new ChartTitle() { Text = "Line Chart", Alignment = StringAlignment.Center });


            using (MemoryStream s = new MemoryStream())
            {
                lineChart.ExportToImage(s, ImageFormat.Png);
                return Image.FromStream(s);
            }
        }
    }

    /// <summary>
    /// Данные линии
    /// </summary>
    public class LineData
    {
        /// <summary>
        /// Название линии
        /// </summary>
        public string NameLine { get; set; } = string.Empty;

        /// <summary>
        /// Список точек линии
        /// </summary>
        public IEnumerable<LinePoint> LinePoints { get; set; } = Array.Empty<LinePoint>();
    }

    /// <summary>
    /// Данные точки по оси X & Y
    /// </summary>
    public class LinePoint
    {
        public double XValue { get; set; }
        public double YValue { get; set; }
    }
}
