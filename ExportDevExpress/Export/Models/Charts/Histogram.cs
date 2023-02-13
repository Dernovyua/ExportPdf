using Export.DrawingCharts;
using Export.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Export.Models.Charts
{
    /// <summary>
    /// Гистограмма
    /// </summary>
    public class Histogram : IChart
    {
        public IEnumerable<double> HistrogramData { get; set; } = Array.Empty<double>();
        public SettingChart SettingChart { get; set; }

        private HistogramETS histogramETS { get; set; }

        public Histogram(IEnumerable<double> histrogramData, SettingChart settingChart)
        {
            HistrogramData = histrogramData;

            SettingChart = settingChart;

            histogramETS = new HistogramETS(new HistrogramEtsSettings()
            {
                _data = histrogramData.ToList(),

                _mapH = settingChart.Height,
                _mapW = settingChart.Width,

                xText = settingChart.SignatureX,
                yText = settingChart.SignatureY,

                _chartColor = settingChart.ChartColor,

                _markerColor = settingChart.MarkerSetting.MarkerColor,
                _markerCount = settingChart.MarkerSetting.MarkerCount,
                _markerStart = settingChart.MarkerSetting.MarkerStart,
            });
        }

        public Image CreateImage()
        {
            histogramETS.DrawChart();

            return histogramETS._bmp;
        }
    }
}
