using DevExpress.Utils;
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

        public SettingChart SettingChart { get; set; }

        public Area(IEnumerable<AreaData> areas, SettingChart settingChart)
        {
            Areas = areas;
            SettingChart = settingChart;
        }
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
                    series[^1].Points.Add(new SeriesPoint(item.XValue, item.YValue));
                }

                series[^1].ArgumentScaleType = ScaleType.Numerical;

                ((AreaSeriesView)series[^1].View).MarkerVisibility = DefaultBoolean.True;
                ((AreaSeriesView)series[^1].View).Transparency = 80;
            }

            areaChart.Series.AddRange(series.ToArray());

            ((XYDiagram)areaChart.Diagram).EnableAxisXZooming = true;
            ((XYDiagram)areaChart.Diagram).AxisY.Interlaced = true;
            ((XYDiagram)areaChart.Diagram).AxisY.InterlacedColor = Color.FromArgb(20, 60, 60, 60);
            ((XYDiagram)areaChart.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
            ((XYDiagram)areaChart.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;

            areaChart.Width = SettingChart.Width;
            areaChart.Height = SettingChart.Height;

            areaChart.Titles.Add(new ChartTitle() { Text = SettingChart.Name, Alignment = StringAlignment.Center, TextColor = SettingChart.SettingText.Color });

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
