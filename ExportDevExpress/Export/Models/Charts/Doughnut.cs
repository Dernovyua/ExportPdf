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
    public class Doughnut : IChart
    {
        public IEnumerable<PieData> PieData { get; set; } = Array.Empty<PieData>();
        public SettingChart SettingChart { get; set; }

        public Doughnut(IEnumerable<PieData> pieData, SettingChart settingChart)
        {
            PieData = pieData;
            SettingChart = settingChart;
        }

        public Doughnut()
        {
            
        }

        public Image CreateImageFromControl()
        {
            ChartControl pieChart = new ChartControl();

            Series series = new Series("", ViewType.Doughnut);

            foreach (PieData pie in PieData)
            {
                series.Points.Add(new SeriesPoint(pie.Argument, pie.Value));
            }

            series.ArgumentDataMember = "Argument";
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            series.SeriesPointsSorting = SortingMode.Ascending;
            series.SeriesPointsSortingKey = SeriesPointKey.Argument;

            series.Label.TextPattern = "{A}: {VP:P2}";

            pieChart.Series.Add(series);

            ((DoughnutSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;
            ((DoughnutSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
            ((DoughnutSeriesLabel)series.Label).ResolveOverlappingMinIndent = 5;

            //((DoughnutSeriesView)series.View).ExplodedPoints.Add(series.Points[0]);
            ((DoughnutSeriesView)series.View).ExplodedDistancePercentage = 30;

            ((SimpleDiagram)pieChart.Diagram).Dimension = 2;

            pieChart.Titles.Add(new ChartTitle() 
            { 
                Text = SettingChart.Name, 
                Alignment = StringAlignment.Center,
                TextColor = SettingChart.SettingText.Color,
            });
            pieChart.Legend.Visibility = DefaultBoolean.False;

            pieChart.Width = SettingChart.Width;
            pieChart.Height = SettingChart.Height;

            using (MemoryStream s = new MemoryStream())
            {
                pieChart.ExportToImage(s, ImageFormat.Png);
                return Image.FromStream(s);
            }
        }
    }

    public class PieData
    {
        public string Argument { get; set; } = string.Empty;
        public double Value { get; set; }
    }
}
