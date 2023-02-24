using Export.DrawingCharts;
using Export.Interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace Export.Models.Charts
{
    /// <summary>
    /// Гистограмма
    /// </summary>
    public class Histogram : IChart
    {
        public List<double> HistrogramData { get; set; } = new();
        public SettingChart SettingChart { get; set; }

        private HistogramETS histogramETS { get; set; }

        public Histogram(List<double> histrogramData, int markerStart, int markerCount)
        {
            histogramETS = new HistogramETS(new HistrogramEtsSettings()
            {
                _data = histrogramData,
                _markerCount = markerCount,
                _markerStart = markerStart,
                //xText = "Индекс прохода в текущем цикле",
                //yText = "Индекс прохода в текущем цикле"
            });
        }

        public Histogram(List<double> histrogramData, SettingChart settingChart)
        {
            HistrogramData = histrogramData;

            SettingChart = settingChart;

            histogramETS = new HistogramETS(new HistrogramEtsSettings()
            {
                _data = histrogramData,

                _mapH = settingChart.Height,
                _mapW = settingChart.Width,

                //xText = settingChart.SignatureX,
                //yText = settingChart.SignatureY,

                _chartColor = settingChart.ChartColor,

                _markerColor = settingChart.MarkerSetting.MarkerColor,
                _markerCount = settingChart.MarkerSetting.MarkerCount,
                _markerStart = settingChart.MarkerSetting.MarkerStart,
            });
        }

        public Image CreateChartImage()
        {
            histogramETS.DrawChart();

            return histogramETS._bmp;
        }
    }
}
