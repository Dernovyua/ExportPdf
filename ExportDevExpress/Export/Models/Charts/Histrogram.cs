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
    /// <summary>
    /// Гистограмма
    /// </summary>
    public class Histrogram : IChart
    {
        public IEnumerable<HistrogramData> HistrogramData { get; set; } = Array.Empty<HistrogramData>();
        public IEnumerable<AreaHistrogram> AreasHistrogram { get; set; } = Array.Empty<AreaHistrogram>();
        public SettingChart SettingChart { get; set; }

        public Histrogram(IEnumerable<HistrogramData> histrogramData, IEnumerable<AreaHistrogram> areasHistrogram, SettingChart settingChart)
        {
            HistrogramData = histrogramData;
            AreasHistrogram = areasHistrogram;

            SettingChart = settingChart;
        }

        public Histrogram()
        {

        }

        public Image CreateImageFromControl()
        {
            ChartControl chartControl = new ChartControl();

            #region Создание графика

            Series histogram = new Series("", SettingChart.Dimension.Equals(Dimension.Two) ? ViewType.Bar : ViewType.Bar3D);

            histogram.ArgumentDataMember = "XValue";
            histogram.ValueDataMembers[0] = "YValue";
            histogram.DataSource = HistrogramData;

            chartControl.Height = SettingChart.Height;
            chartControl.Width = SettingChart.Width;

            chartControl.Series.Add(histogram);

            #region Отрисвока выделения областей на гистограмме

            List<Series> seriesArea = new List<Series>();

            foreach (AreaHistrogram areaHistogram in AreasHistrogram)
            {
                seriesArea.Add(new Series("", SettingChart.Dimension.Equals(Dimension.Two) ? ViewType.Area : ViewType.Area3D));

                foreach (HistrogramData dataHistrogram in areaHistogram.AreaHistogramData)
                {
                    seriesArea[^1].Points.Add(new SeriesPoint(dataHistrogram.XValue, dataHistrogram.YValue));
                }

                if (SettingChart.Dimension.Equals(Dimension.Two))
                    ((AreaSeriesView)seriesArea[^1].View).Transparency = 95;
                else
                    ((Area3DSeriesView)seriesArea[^1].View).Transparency = 95;
            }

            chartControl.Series.AddRange(seriesArea.ToArray());

            #endregion

            #endregion

            #region Насйтрока графика

            if (SettingChart.Dimension.Equals(Dimension.Two))
            {
                ((BarSeriesView)histogram.View).AggregateFunction = SeriesAggregateFunction.Default;
                ((BarSeriesView)histogram.View).Color = SettingChart.Color;

                ((BarSeriesView)histogram.View).AxisX.Title.Text = SettingChart.SignatureX;
                ((BarSeriesView)histogram.View).AxisY.Title.Text = SettingChart.SignatureY;

                ((BarSeriesView)histogram.View).AxisX.Title.Visibility = DefaultBoolean.True;
                ((BarSeriesView)histogram.View).AxisY.Title.Visibility = DefaultBoolean.True;

                XYDiagram diagram = (XYDiagram)chartControl.Diagram;

                diagram.AxisX.Visibility = DefaultBoolean.True;
                diagram.AxisY.Title.Text = SettingChart.SignatureY;
                diagram.AxisX.Title.Text = SettingChart.SignatureX;

                diagram.AxisX.GridLines.Visible = true;
                diagram.AxisX.Tickmarks.Visible = true;
            }
            else
            {
                ((Bar3DSeriesView)histogram.View).AggregateFunction = SeriesAggregateFunction.Histogram;
                ((Bar3DSeriesView)histogram.View).Color = SettingChart.Color;

                ((Bar3DSeriesView)histogram.View).BarWidth = 30;
                ((Bar3DSeriesView)histogram.View).BarDepth = 100;

                XYDiagram3D diagram = (XYDiagram3D)chartControl.Diagram;

                diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Continuous;
                diagram.AxisX.NumericScaleOptions.IntervalOptions.DivisionMode = IntervalDivisionMode.Count;
            }

            chartControl.Titles.Add(new ChartTitle() { Text = SettingChart.Name, Alignment = StringAlignment.Center, TextColor = SettingChart.SettingText.Color });

            #endregion

            #region Экспорт контрола в Image формата Png

            using (MemoryStream s = new MemoryStream())
            {
                chartControl.ExportToImage(s, ImageFormat.Png);
                return Image.FromStream(s);
            }

            #endregion
        }
    }

    /// <summary>
    /// Данные для построения диаграммы
    /// </summary>
    public class HistrogramData
    {
        /// <summary>
        /// Данные по оси X
        /// </summary>
        public double XValue { get; set; }

        /// <summary>
        /// Данные по оси Y
        /// </summary>
        public double YValue { get; set; }

        public HistrogramData(double xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
    }

    /// <summary>
    /// Класс для выделения областей на гистограмме
    /// </summary>
    public class AreaHistrogram
    {
        public IEnumerable<HistrogramData> AreaHistogramData { get; set; } = Array.Empty<HistrogramData>();
    }
}
