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
    public class Histrogram : IChart
    {
        public IEnumerable<HistrogramData> HistrogramData { get; set; } = Array.Empty<HistrogramData>();
        public SettingChart SettingChart { get; set; }

        public Histrogram(IEnumerable<HistrogramData> histrogramData, SettingChart settingChart)
        {
            HistrogramData = histrogramData;
            SettingChart = settingChart;
        }

        public Histrogram()
        {
            
        }

        public Image CreateImageFromControl()
        {
            ChartControl chartControl = new ChartControl();

            Series histogram = new Series("", ViewType.Bar);
            histogram.ArgumentDataMember = "XValue";
            histogram.ValueDataMembers[0] = "YValue";
            histogram.DataSource = HistrogramData;

            chartControl.Height = SettingChart.Height;
            chartControl.Width = SettingChart.Width;

            chartControl.Series.Add(histogram);

            ((BarSeriesView)histogram.View).AggregateFunction = SeriesAggregateFunction.Histogram;
            ((BarSeriesView)histogram.View).AxisX.Title.Text = SettingChart.SignatureX;
            ((BarSeriesView)histogram.View).AxisY.Title.Text = SettingChart.SignatureY;

            ((BarSeriesView)histogram.View).AxisX.Title.Visibility = DefaultBoolean.True;
            ((BarSeriesView)histogram.View).AxisY.Title.Visibility = DefaultBoolean.True;

            ((BarSeriesView)histogram.View).Color = SettingChart.Color;

            XYDiagram diagram = (XYDiagram)chartControl.Diagram;

            diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Interval;
            diagram.AxisX.Visibility = DefaultBoolean.True;
            diagram.AxisY.Title.Text = SettingChart.SignatureY;
            diagram.AxisX.Title.Text = SettingChart.SignatureX;

            diagram.AxisX.NumericScaleOptions.IntervalOptions.DivisionMode = IntervalDivisionMode.Auto;

            chartControl.Titles.Add(new ChartTitle() { Text = SettingChart.Name, Alignment = StringAlignment.Center, TextColor = SettingChart.SettingText.Color });

            using (MemoryStream s = new MemoryStream())
            {
                chartControl.ExportToImage(s, ImageFormat.Png);
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
