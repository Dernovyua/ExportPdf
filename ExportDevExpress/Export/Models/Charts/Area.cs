using DevExpress.XtraCharts;
using Export.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Export.Models.Charts
{
    public class Area : IChart
    {
        public IEnumerable<AreaData> Areas { get; set; } = Array.Empty<AreaData>();

        public Area()
        {
            
        }

        public Image CreateImageFromControl()
        {
            ChartControl areaChart = new ChartControl();

            List<Series> series = new List<Series>();

            foreach (AreaData area in Areas)
            {
                series.Add(new Series(area.NameArea, ViewType.Area));

                foreach (var item in area.AreaPoints)
                {
                    series[series.Count - 1].Points.Add(new SeriesPoint(item.XValue, item.YValue));
                }

                series[series.Count - 1].ArgumentScaleType = ScaleType.Numerical;
            }

            areaChart.Series.AddRange(series.ToArray());

            areaChart.Width = 600;
            areaChart.Height = 350;

            ((XYDiagram)areaChart.Diagram).EnableAxisXZooming = true;

            using (MemoryStream s = new MemoryStream())
            {
                areaChart.ExportToImage(s, ImageFormat.Png);
                return Image.FromStream(s);
            }
        }
    }

    public class AreaData
    {
        public string NameArea { get; set; } = string.Empty;
        public IEnumerable<AreaPoint> AreaPoints { get; set; } = Array.Empty<AreaPoint>();
    }

    public class AreaPoint
    {
        public double XValue { get; set; }
        public double YValue { get; set; }
    }
}
