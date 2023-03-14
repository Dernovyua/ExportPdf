using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace Export.DrawingCharts
{
    public class LineETS
    {
        private Bitmap _bmp { get; set; }
        LineChartSet _chartSet = null;
        List<LineChartData> _data;
        Graphics _g;
        float _border = 0;
        float _wChart = 0;
        float _hChart = 0;
        float _step = 0;
        float _barDelta = 0;
        Matrix _original = null;

        // данные нормировки 
        double _normX = 0;
        double _offX = 0;
        double _normY = 0;
        double _offY = 0;
        /// <summary>
        ///  Конструктор
        /// </summary>

        public LineETS(LineChartSet set, List<LineChartData> data)
        {
            _chartSet = set;
            _data = data;
            _bmp = new Bitmap(Convert.ToInt32(_chartSet._mapW), Convert.ToInt32(_chartSet._mapH), PixelFormat.Format32bppArgb);
            _g = Graphics.FromImage(_bmp);
            Normalize();
            // Расчет border
            Font font1 = new Font("Arial", _chartSet.markerFontSize, FontStyle.Regular);
            Font font2 = new Font("Arial", _chartSet.axisFontSize, FontStyle.Regular);
            string maxValue = Math.Round(_normY, 2).ToString();
            SizeF size1 = _g.MeasureString(maxValue, font1);
            SizeF size2 = _g.MeasureString(_chartSet.yText, font2);
            _border = (float)(size1.Width + size2.Height + 5f);
            _wChart = _chartSet._mapW - _border * 2;
            _hChart = _chartSet._mapH - _border * 2;
            _original = _g.Transform;
            Matrix m = new Matrix(1, 0, 0, -1, 0, (float)_chartSet._mapH);
            _g.Transform = m;
            _g.Clear(Color.White);
        }

        /// <summary>
        ///  Возвращаем картинку
        /// </summary>
        public Image GetImage() { return _bmp; }
        /// <summary>
        ///  Расчет X в экранных координатах
        /// </summary>
        float ScrX(double x) { return (float)((x - _offX) * _wChart + _border); }
        /// <summary>
        ///  Расчет Y в экранных координатах
        /// </summary>
        float ScrY(double y) { return (float)((y - _offY) * _hChart + _border); }
        /// <summary>
        ///  Маркировка по оси Y
        /// </summary>
        void DrawYMarker(float y, double value, SolidBrush brush) {
            Matrix m = _g.Transform;
            _g.Transform = _original;
            Font font = new Font("Arial", _chartSet.markerFontSize, FontStyle.Regular);
            string strValue = Convert.ToString(Math.Round(value, 2));
            SizeF size = _g.MeasureString(strValue, font);
            float scrX = _border - size.Width - 3f;
            float scrY = (float)(_bmp.Height - y);
            _g.DrawString(strValue, font, brush, scrX, scrY);
            _g.Transform = m;
        }
        /// <summary>
        /// рисуем временную метку по оси X 
        /// </summary>
        void DawXMarker(float x, DateTime dt, SolidBrush brush) {
            Matrix m = _g.Transform;
            _g.Transform = _original;
            Font font = new Font("Arial", _chartSet.markerFontSize, FontStyle.Regular);
            string strVal = dt.Date.ToShortDateString();
            SizeF size = _g.MeasureString(_chartSet.yText, font);
            float scrX = x - size.Width / 2;
            float scrY = _bmp.Height - _border + 6f;
            _g.DrawString(strVal, font, brush, scrX, scrY);
            _g.Transform = m;
        }
        /// <summary>
        ///  Подпись вертикальной оси
        /// </summary>
        void SignYAxis() {
            Matrix m = _g.Transform;
            _g.Transform = _original;
            Font font = new Font("Arial", _chartSet.axisFontSize, FontStyle.Regular);
            SizeF size = _g.MeasureString(_chartSet.yText, font);
            float x = 3f;
            float y = _bmp.Height / 2 + size.Width / 2;
            _g.TranslateTransform(x, y);
            _g.RotateTransform(-90f);
            _g.DrawString(_chartSet.yText, font, new SolidBrush(Color.Black), 0, 0);
            _g.Transform = m;
        }
        /// <summary>
        ///  Подпись горизонтальной оси
        /// </summary>
        void SignXAxis() {
            Matrix m = _g.Transform;
            _g.Transform = _original;
            Font font = new Font("Arial", _chartSet.axisFontSize, FontStyle.Regular);
            SizeF size = _g.MeasureString(_chartSet.xText, font);
            float x = _bmp.Width / 2 - size.Width / 2;
            float y = _bmp.Height - (size.Height + 10f);
            _g.DrawString(_chartSet.xText, font, new SolidBrush(Color.Black), x, y);
            _g.Transform = m;
        }
        /// <summary>
        ///  Оси, сетка, ....
        /// </summary>
        public void DrawAxis() {
            Pen grayPen = new Pen(Color.LightGray, 1);
            Pen blackPen = new Pen(Color.Black, 1);
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            // рисуем границу
            _g.DrawLine(blackPen, _border, _border, _border, (float)_bmp.Height - _border / 2);
            _g.DrawLine(blackPen, _border, _border, (float)_bmp.Width - _border / 2, _border);
            // Рисуем X
            _g.DrawLine(blackPen, ScrX(0), ScrY(0), (float)_bmp.Width - _border / 2, ScrY(0));

            // рисуем горизонтальную сетку
            double step = 0.2;

            for (int i = 0; i < (int)1 / step; i++) {
                double upY = ScrY(step * (i + 1));
                double downY = ScrY(-step * (i + 1));

                if (upY < _border + _hChart) {
                    double yValue = step * (i + 1);
                    float scrY = ScrY(yValue);
                    _g.DrawLine(grayPen, ScrX(0), scrY, (float)_bmp.Width - _border / 2, scrY);
                    DrawYMarker(scrY, yValue * _normY, blackBrush);
                }

                if (downY > _border) {
                    double yValue = -step * (i + 1);
                    float scrY = ScrY(yValue);
                    _g.DrawLine(grayPen, ScrX(0), scrY, (float)_bmp.Width - _border / 2, scrY);
                    DrawYMarker(scrY, yValue * _normY, blackBrush);
                }
            }
            SignYAxis();

            // Разметкаа на горизонтальной оси
            step = 0.25;
            float y1 = _border + 5f;
            float y2 = _border - 5f;

            for (int i = 0; i < (int)1 / step; i++) {
                float x = ScrX(step * (i + 1));
                _g.DrawLine(blackPen, x, y1, x, y2);
                int dtIdx = (int)((step * (i + 1)) * _normX);

                if (dtIdx >= 0 && dtIdx < _chartSet.date.Count)
                    DawXMarker(x, _chartSet.date[dtIdx], blackBrush);
            }
            SignXAxis();
        }
        /// <summary>
        ///  рисуем график0
        /// </summary>
        public void Draw() {
            DrawAxis();

            for (int i = 0; i < _data.Count; i++) {
                List<Point> points = _data[i].points;
                Pen pen = new Pen(_data[i].color, _data[i].width);
                Pen redPen = new Pen(Color.Red, _data[i].width);

                for (int j = 0; j < points.Count - 1; j++) {
                    if (points[j + 1].Y >= 0)
                        _g.DrawLine(pen, ScrX(points[j].X), ScrY(points[j].Y), ScrX(points[j + 1].X), ScrY(points[j + 1].Y));
                    else
                        _g.DrawLine(redPen, ScrX(points[j].X), ScrY(points[j].Y), ScrX(points[j + 1].X), ScrY(points[j + 1].Y));
                }
            }
        }
        /// <summary>
        ///  Нормируем параметры
        /// </summary>
        void Normalize() {
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (LineChartData data in _data) {
                List<Point> points = data.points;
                double min = points.Min(a => a.X);

                if (min < minX)
                    minX = min;
                double max = points.Max(a => a.X);

                if (max > maxX)
                    maxX = max;
                min = points.Min(a => a.Y);

                if (min < minY)
                    minY = min;
                max = points.Max(a => a.Y);

                if (max > maxY)
                    maxY = max;
            }

            _normX = maxX - minX;
            _normY = maxY - minY;
            _offX = minX / _normX;
            _offY = minY / _normY;

            foreach (LineChartData data in _data) {
                foreach (Point point in data.points) {
                    point.X /= _normX;
                    point.Y /= _normY;
                }
            }
        }
        public class Point
        {
            public double X { get; set; } = 0;
            public double Y { get; set; } = 0;

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        public class LineChartData
        {
            public List<Point> points = new List<Point>();
            public Color color = Color.Blue;
            public int width = 1;
        }

        public class LineChartSet
        {
            public int _mapW = 700;
            public int _mapH = 350;
            public string xText = "Доходность";
            public string yText = "Период времени";
            public int axisFontSize = 8;
            public int markerFontSize = 8;
            public List<DateTime> date = new List<DateTime>();
        }
    }
}
