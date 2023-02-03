using DevExpress.Utils;
using DevExpress.XtraCharts;
using Export.Enums;
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
        /// <summary>
        /// Список данных для построения графика
        /// </summary>
        public IEnumerable<DoughnutData> DoughnutData { get; set; } = Array.Empty<DoughnutData>();

        /// <summary>
        /// Настройки графика
        /// </summary>
        public SettingChart SettingChart { get; set; }

        public Doughnut(IEnumerable<DoughnutData> doughnutData, SettingChart settingChart)
        {
            DoughnutData = doughnutData;
            SettingChart = settingChart;
        }

        public Doughnut()
        {

        }

        public Image CreateImageFromControl()
        {
            ChartControl pieChart = new ChartControl();

            #region Создание графика и добавление в контрол

            Series series = new Series("", SettingChart.Dimension.Equals(Dimension.Two) ? ViewType.Doughnut : ViewType.Doughnut3D);

            #region Заполнение данными

            foreach (DoughnutData pie in DoughnutData)
            {
                series.Points.Add(new SeriesPoint(pie.Argument, pie.Value));
            }

            #endregion

            series.ArgumentDataMember = "Argument";
            series.ValueDataMembers.AddRange(new string[] { "Value" });

            series.SeriesPointsSorting = SortingMode.Ascending;
            series.SeriesPointsSortingKey = SeriesPointKey.Argument;

            series.Label.TextPattern = "{A}:{VP:P2}";

            pieChart.Series.Add(series);

            #endregion

            #region Насйтроки графика

            if (SettingChart.Dimension.Equals(Dimension.Two))
            {
                ((DoughnutSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;
                ((DoughnutSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                ((DoughnutSeriesLabel)series.Label).ResolveOverlappingMinIndent = 5;

                ((DoughnutSeriesView)series.View).ExplodedDistancePercentage = 30;

                ((SimpleDiagram)pieChart.Diagram).Dimension = 2;
            }
            else
            {
                ((Doughnut3DSeriesLabel)series.Label).Position = PieSeriesLabelPosition.TwoColumns;
                ((Doughnut3DSeriesLabel)series.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                ((Doughnut3DSeriesLabel)series.Label).ResolveOverlappingMinIndent = 5;

                ((Doughnut3DSeriesView)series.View).ExplodedDistancePercentage = 30;

                ((SimpleDiagram3D)pieChart.Diagram).Dimension = 3;
            }

            pieChart.Titles.Add(new ChartTitle() 
            { 
                Text = SettingChart.Name, 
                Alignment = StringAlignment.Center,
                TextColor = SettingChart.SettingText.Color,
            });
            pieChart.Legend.Visibility = DefaultBoolean.False;

            pieChart.Width = SettingChart.Width;
            pieChart.Height = SettingChart.Height;

            #endregion

            #region Экспорт контрола в Image формата Png

            using (MemoryStream s = new MemoryStream())
            {
                pieChart.ExportToImage(s, ImageFormat.Png);
                return Image.FromStream(s);
            }

            #endregion
        }
    }

    /// <summary>
    /// Данные графика
    /// </summary>
    public class DoughnutData
    {
        /// <summary>
        /// По оси X
        /// </summary>
        public string Argument { get; set; } = string.Empty;

        /// <summary>
        /// По оси Y
        /// </summary>
        public double Value { get; set; }
    }
}
