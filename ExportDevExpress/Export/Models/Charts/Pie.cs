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
    public class Pie : IChart
    {
        public IEnumerable<PieData> PieData { get; set; } = Array.Empty<PieData>();
        public SettingChart SettingChart { get; set; }

        public Pie(IEnumerable<PieData> pieData, SettingChart settingChart)
        {
            PieData = pieData;
            SettingChart = settingChart;
        }

        public Pie()
        {
            
        }

        public Image CreateImageFromControl()
        {
            ChartControl pieChart = new ChartControl();

            Series series = new Series("", ViewType.Pie);

            series.ArgumentDataMember = "Argument";
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            series.LegendTextPattern = "{A}";
            series.DataSource = PieData;

            series.LabelsVisibility = DefaultBoolean.True;

            pieChart.Width = SettingChart.Width;
            pieChart.Height = SettingChart.Height;

            pieChart.Titles.Add(new ChartTitle() { Text = SettingChart.Name });

            pieChart.Series.Add(series);

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
