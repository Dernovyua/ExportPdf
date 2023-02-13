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
    public class Line : IChart
    {
        /// <summary>
        /// Список линий, которые необходимо построить
        /// </summary>
        public IEnumerable<LineData> Lines { get; set; } = Array.Empty<LineData>();

        public SettingChart SettingChart { get; set; }

        public Line()
        {
            SettingChart = new SettingChart();
        }

        public Image CreateImage()
        {
            //ChartControl lineChart = new ChartControl();

            //List<Series> series = new List<Series>();

            //#region Создание серий (графиков Line) и добавление в контрол

            //foreach (LineData line in Lines)
            //{
            //    series.Add(new Series(line.NameLine, SettingChart.Dimension.Equals(Dimension.Two) ? ViewType.Line : ViewType.Line3D));

            //    #region Добавление серии (графика Line) данными

            //    foreach (var item in line.LinePoints)
            //    {
            //        series[^1].Points.Add(new SeriesPoint(item.XValue, item.YValue));
            //    }

            //    #endregion

            //    series[series.Count - 1].ArgumentScaleType = ScaleType.Numerical;

            //    if (SettingChart.Dimension.Equals(Dimension.Two))
            //    {
            //        ((LineSeriesView)series[^1].View).MarkerVisibility = DefaultBoolean.True;
            //        ((LineSeriesView)series[^1].View).LineMarkerOptions.Size = 5;
            //        ((LineSeriesView)series[^1].View).LineMarkerOptions.Kind = MarkerKind.Circle;
            //        ((LineSeriesView)series[^1].View).LineStyle.DashStyle = DashStyle.Dash;
            //    }
            //    else
            //    {
            //        ((Line3DSeriesView)series[^1].View).LineWidth = 5;
            //        ((Line3DSeriesView)series[^1].View).LineThickness = 1;
            //    }
            //}

            //lineChart.Series.AddRange(series.ToArray());

            //#endregion

            //#region Настройка графика

            //if (SettingChart.Dimension.Equals(Dimension.Two)) // при двумерном графике
            //{
            //    ((XYDiagram)lineChart.Diagram).AxisY.Interlaced = true;
            //    ((XYDiagram)lineChart.Diagram).AxisY.InterlacedColor = Color.FromArgb(20, 60, 60, 60);
            //    ((XYDiagram)lineChart.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
            //    ((XYDiagram)lineChart.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;

            //    ((XYDiagram)lineChart.Diagram).AxisX.Title.Visibility = DefaultBoolean.True;
            //    ((XYDiagram)lineChart.Diagram).AxisY.Title.Visibility = DefaultBoolean.True;

            //    ((XYDiagram)lineChart.Diagram).AxisX.Title.Text = SettingChart.SignatureX;
            //    ((XYDiagram)lineChart.Diagram).AxisY.Title.Text = SettingChart.SignatureY;
            //}
            //else // при трехмерном
            //{
            //    ((XYDiagram3D)lineChart.Diagram).AxisY.Interlaced = true;
            //    ((XYDiagram3D)lineChart.Diagram).AxisY.InterlacedColor = Color.FromArgb(20, 60, 60, 60);
            //    ((XYDiagram3D)lineChart.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
            //    ((XYDiagram3D)lineChart.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;
            //}

            //lineChart.Width = SettingChart.Width;
            //lineChart.Height = SettingChart.Height;

            //lineChart.Titles.Add(new ChartTitle() { Text = SettingChart.Name, Alignment = StringAlignment.Center, TextColor = SettingChart.SettingText.Color });

            //#endregion

            //#region Экспорт контрола в Image формата Png

            //using (MemoryStream s = new MemoryStream())
            //{
            //    lineChart.ExportToImage(s, ImageFormat.Png);
            //    return Image.FromStream(s);
            //}

            //#endregion

            return null;
        }
    }

    /// <summary>
    /// Данные линии
    /// </summary>
    public class LineData
    {
        /// <summary>
        /// Название линии
        /// </summary>
        public string NameLine { get; set; } = string.Empty;

        /// <summary>
        /// Список точек линии
        /// </summary>
        public IEnumerable<LinePoint> LinePoints { get; set; } = Array.Empty<LinePoint>();
    }

    /// <summary>
    /// Данные точки по оси X & Y
    /// </summary>
    public class LinePoint
    {
        /// <summary>
        /// Данные по оси X
        /// </summary>
        public double XValue { get; set; }

        /// <summary>
        /// Данные по оси X
        /// </summary>
        public double YValue { get; set; }
    }
}
