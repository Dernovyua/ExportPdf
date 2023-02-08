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
    public class Area : IChart
    {
        /// <summary>
        /// Список графиков Area
        /// </summary>
        public IEnumerable<AreaData> Areas { get; set; } = Array.Empty<AreaData>();

        /// <summary>
        /// Настройки графика
        /// </summary>
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
                series.Add(new Series(area.NameArea, SettingChart.Dimension.Equals(Dimension.Two) ? ViewType.Area : ViewType.Area3D));

                foreach (var item in area.AreaPoints)
                {
                    series[^1].Points.Add(new SeriesPoint(item.XValue, item.YValue));
                }

                series[^1].ArgumentScaleType = ScaleType.Numerical;

                if (SettingChart.Dimension.Equals(Dimension.Two))
                {
                    ((AreaSeriesView)series[^1].View).MarkerVisibility = DefaultBoolean.True;
                    ((AreaSeriesView)series[^1].View).Transparency = 80;
                }
                else
                    ((Area3DSeriesView)series[^1].View).Transparency = 80;
            }

            areaChart.Series.AddRange(series.ToArray());

            if (SettingChart.Dimension.Equals(Dimension.Two))
            {
                ((XYDiagram)areaChart.Diagram).EnableAxisXZooming = true;
                ((XYDiagram)areaChart.Diagram).AxisY.Interlaced = true;
                ((XYDiagram)areaChart.Diagram).AxisY.InterlacedColor = Color.FromArgb(20, 60, 60, 60);
                ((XYDiagram)areaChart.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
                ((XYDiagram)areaChart.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;

                ((XYDiagram)areaChart.Diagram).AxisX.Title.Visibility = DefaultBoolean.True;
                ((XYDiagram)areaChart.Diagram).AxisY.Title.Visibility = DefaultBoolean.True;

                ((XYDiagram)areaChart.Diagram).AxisX.Title.Text = SettingChart.SignatureX;
                ((XYDiagram)areaChart.Diagram).AxisY.Title.Text = SettingChart.SignatureY;
            }
            else
            {
                ((XYDiagram3D)areaChart.Diagram).AxisY.Interlaced = true;
                ((XYDiagram3D)areaChart.Diagram).AxisY.InterlacedColor = Color.FromArgb(20, 60, 60, 60);
                ((XYDiagram3D)areaChart.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
                ((XYDiagram3D)areaChart.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;
            }

            areaChart.Width = SettingChart.Width;
            areaChart.Height = SettingChart.Height;

            areaChart.Titles.Add(new ChartTitle() { Text = SettingChart.Name, Alignment = StringAlignment.Center, TextColor = SettingChart.SettingText.Color });

            #region Экспорт контрола в Image формата Png

            using (MemoryStream s = new MemoryStream())
            {
                areaChart.ExportToImage(s, ImageFormat.Png);
                return Image.FromStream(s);
            }

            #endregion
        }
    }

    /// <summary>
    /// Данные для построения Area
    /// </summary>
    public class AreaData
    {
        /// <summary>
        /// Название графика
        /// </summary>
        public string NameArea { get; set; } = string.Empty;

        /// <summary>
        /// Список точек по осям
        /// </summary>
        public IEnumerable<AreaPoint> AreaPoints { get; set; } = Array.Empty<AreaPoint>();
    }

    /// <summary>
    /// Точки Area по оси X & Y
    /// </summary>
    public class AreaPoint
    {
        /// <summary>
        /// Данные по оси X
        /// </summary>
        public double XValue { get; set; }

        /// <summary>
        /// Данные по оси Y
        /// </summary>
        public double YValue { get; set; }

        public AreaPoint(double xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }

        public AreaPoint()
        {
            
        }
    }
}
