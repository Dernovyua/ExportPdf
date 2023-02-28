using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Export.DrawingCharts
{
    public class HistogramETS
    {
        HistrogramEtsSettings _chartSet = null;
        private Bitmap _bmp { get; set; }
        public List<float> _chartData;
        Graphics _g;
        float _kBorder = 0.075f;
        float _border = 0;
        float _wChart = 0;
        float _hChart = 0;
        float _step = 0;
        float _barDelta = 0;
        Matrix _original = null;

        public HistogramETS(HistrogramEtsSettings chartSet)
        {
            _chartData = new List<float>();
            _chartSet = chartSet;
            _bmp = new Bitmap(Convert.ToInt32(chartSet._mapW), Convert.ToInt32(chartSet._mapH), PixelFormat.Format32bppArgb);
            _g = Graphics.FromImage(_bmp);
            _original = _g.Transform;
            Matrix m = new Matrix(1.0f, 0.0f, 0.0f, -1.0f, 0.0f, (float)chartSet._mapH);
            _g.Transform = m;
            _g.Clear(Color.White);
            _border = (float)chartSet._mapW * _kBorder;
            _wChart = chartSet._mapW - _border * 2;
            _hChart = chartSet._mapH - _border * 2;

            // Предполагаем, что данные приходят в нормадизованном виде 
            foreach (float x in chartSet._data)
            {
                _chartData.Add(x * _hChart);
            }
            _step = _wChart / _chartData.Count;
            _barDelta = _step * 0.2f;

            if (_barDelta <= 0)
                _barDelta = 1;
        }

        private void DrawAxis()
        {
            // рисуем Оси
            _g.DrawLine(new Pen(Color.Black, 2), _border, _border, _border, (float)_bmp.Height - _border / 2);
            _g.DrawLine(new Pen(Color.Black, 2), _border, _border, (float)_bmp.Width - _border / 2, _border);
            // Рисуем сетку
            float x1 = _border;
            float x2 = (float)_bmp.Width - _border / 2;
            float yOriginal = 0.25f;
            float y12 = (float)(_border + yOriginal * _hChart);

            for (int i = 0; i < 4; i++)
            {
                _g.DrawLine(new Pen(Color.LightGray, 1), x1, y12, x2, y12);
                yOriginal += 0.25f;
                y12 = (float)(_border + yOriginal * _hChart);
            }
            // Маркировка горизонтальной оси
            float x = _border + 3f;
            float y = _bmp.Height - (_border - 3f);
            float markerStepMin = 30;
            int markerStep = 1;

            if (_step < markerStepMin)
                markerStep = (int)Math.Round(markerStepMin / (float)_step, 0);

            Matrix m = _g.Transform;
            _g.Transform = _original;
            Font font = new Font(_chartSet.FontName, _chartSet.FontSize, FontStyle.Regular);

            for (int i = 0; i < _chartData.Count; i += markerStep)
            {
                _g.DrawString(i.ToString(), font, new SolidBrush(Color.Black), x, y);
                x += markerStep * _step;
            }
            // Маркировка вертикальной оси
            yOriginal = 0.25f;
            x = _border - 25f;
            y = _bmp.Height - (float)(_border + yOriginal * _hChart);

            for (int i = 0; i < 4; i++)
            {
                _g.DrawString(yOriginal.ToString(), font, new SolidBrush(Color.Black), x, y);
                yOriginal += 0.25f;
                y = _bmp.Height - (float)(_border + yOriginal * _hChart);
            }
            // Подпись горизонтальной оси
            Font fontSigne = new Font(_chartSet.FontName, _chartSet.FontSize, FontStyle.Regular);
            SizeF size = _g.MeasureString(_chartSet.xText, fontSigne);
            x = _bmp.Width / 2 - size.Width / 2;
            y = _bmp.Height - (size.Height + 10f);
            _g.DrawString(_chartSet.xText, fontSigne, new SolidBrush(_chartSet.Color), x, y);
            // Подпись вертикальной оси
            size = _g.MeasureString(_chartSet.yText, fontSigne);
            x = 3f;
            y = _bmp.Height / 2 + size.Width / 2;
            _g.TranslateTransform(x, y);
            _g.RotateTransform(-90f);
            _g.DrawString(_chartSet.yText, fontSigne, new SolidBrush(_chartSet.Color), 0, 0);
            // Восстанавливаем матрицу
            _g.Transform = m;
        }
        private void DrawChart()
        {
            DrawAxis();
            float x = _border + 3f;
            float y = _border + 3f;
            int mEnd = _chartSet._markerStart + _chartSet._markerCount;

            for (int i = 0; i < _chartData.Count; i++)
            {
                SolidBrush brush = new SolidBrush(_chartSet._chartColor);

                if (_chartSet._markerCount > 0 && i >= _chartSet._markerStart && i < mEnd)
                    brush = new SolidBrush(_chartSet._markerColor);
                float w = _step - _barDelta;

                _g.FillRectangle(brush, x, y, w, _chartData[i]);
                x += _step;
            }
        }

        public Image GetImage()
        {
            DrawChart();

            return _bmp;
        }
    }

    public class HistrogramEtsSettings
    {
        public string FontName { get; set; } = "Arial";
        public float FontSize { get; set; } = 8;
        public Color Color { get; set; } = Color.Black;

        public int _mapW = 700;
        public int _mapH = 350;
        public List<Double> _data;
        public int _markerStart = 0;
        public int _markerCount = 0;
        public Color _chartColor = Color.Blue;
        public Color _markerColor = Color.Red;
        public string xText = "Индекс прохода в текущем цикле";
        public string yText = "Индекс прохода в текущем цикле";

        public HistrogramEtsSettings()
        {
            _data = new List<Double>();
            _chartColor = Color.Blue;
            _markerColor = Color.Red;
        }
    }
}
