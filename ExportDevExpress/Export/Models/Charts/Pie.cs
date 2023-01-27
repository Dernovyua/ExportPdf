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

            Series series1 = new Series(SettingChart.Name, ViewType.Pie);

            series1.ArgumentDataMember = "Argument";
            series1.ValueDataMembers.AddRange(new string[] { "Value" });

            series1.LegendTextPattern = "{A}";
            series1.DataSource = PieData;

            series1.LabelsVisibility = DefaultBoolean.True;

            pieChart.Width = SettingChart.Width;
            pieChart.Height = SettingChart.Height;

            pieChart.Titles.Add(new ChartTitle() { Text = "Pie Chart" });

            pieChart.Series.Add(series1);

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
