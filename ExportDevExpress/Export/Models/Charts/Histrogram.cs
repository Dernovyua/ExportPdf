﻿using DevExpress.XtraCharts;
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

            Series histogram = new Series(SettingChart.Name, ViewType.Bar);
            histogram.ArgumentDataMember = "XValue";
            histogram.ValueDataMembers[0] = "YValue";

            chartControl.Height = SettingChart.Height;
            chartControl.Width = SettingChart.Width;

            chartControl.Series.Add(histogram);

            ((BarSeriesView)histogram.View).AggregateFunction = SeriesAggregateFunction.Histogram;

            histogram.DataSource = HistrogramData;

            XYDiagram diagram = (XYDiagram)chartControl.Diagram;

            diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Interval;
            diagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisX.WholeRange.SideMarginsValue = 0;
            diagram.AxisX.NumericScaleOptions.IntervalOptions.Count = 20;
            diagram.AxisX.NumericScaleOptions.IntervalOptions.DivisionMode = IntervalDivisionMode.Auto;
            diagram.Margins.All = 10;

            diagram.AxisX.Label.TextPattern = "{}{OB}{A1:F1}, {A2:F1}{CB}";

            diagram.AxisX.Title.Text = SettingChart.SignatureX;
            diagram.AxisY.Title.Text = SettingChart.SignatureY;

            chartControl.Titles.Add(new ChartTitle() { Text = "CHART", Alignment = StringAlignment.Center });

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