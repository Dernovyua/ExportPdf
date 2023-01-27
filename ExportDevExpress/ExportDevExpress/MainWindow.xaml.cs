using DevExpress.XtraPrinting;
using DevExpress.XtraSpreadsheet.Model.ModelChart;
using Export;
using Export.Enums;
using Export.Models;
using Export.Models.Charts;
using Export.ModelsExport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace ExportDevExpress
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientReport clientReport { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            clientReport = new ClientReport();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            clientReport.SetExport(new Pdf());

            List<Action> actions = new List<Action>()
            {
                () => clientReport.AddText(new Text("Текстовый блок", new SettingText() { Bold = true, FontSize = 18.0f, TextAligment = Aligment.Center })),
                () => clientReport.AddText(new Text("Оценка параметров оптимизации с учетом плотности распределения статистической оценки", new SettingText() { FontSize = 14.0f, TextAligment = Aligment.Justify })),

                () => clientReport.AddTable(GetTableData()),
                () => clientReport.AddTable(GetTableData2()),
                () => clientReport.AddTable(GetTableData3()),

                () => clientReport.AddChart(new Chart(new Histrogram() { HistrogramData = GetDataHistogram(), SettingChart = new SettingChart("Гистограмма", signatureX: "X", signatureY: "Y") })),
                () => clientReport.AddChart(new Chart(new Histrogram() { HistrogramData = GetDataHistogram(), SettingChart = new SettingChart("Гистограмма2", signatureX: "Hi", signatureY: "Bye") })),
                () => clientReport.AddChart(new Chart(new Pie() { PieData = GetPieData(), SettingChart = new SettingChart("Pie", 600, 350, "X", "Y")})),
                () => clientReport.AddChart(new Chart(new Area()
                {
                    Areas = new List<AreaData>()
                    {
                        new AreaData() { NameArea = "Первый", AreaPoints = GetAreaPoints() },
                        new AreaData() { NameArea = "Второй", AreaPoints = GetAreaPoints2() },
                        new AreaData() { NameArea = "Третий", AreaPoints = GetAreaPoints3() }
                    }
                })),
                () => clientReport.AddText(new Text("\n")),
                () => clientReport.AddText(new Text("Оценка параметров оптимизации с учетом плотности распределения статистической оценки", new SettingText() { Italic = true, FontSize = 14.0f, TextAligment = Aligment.Justify })),
                () => clientReport.AddChart(new Chart(new Line()
                {
                    Lines = new List<LineData>()
                    {
                        new LineData() { NameLine = "Первый", LinePoints = GetLinePoints() },
                        new LineData() { NameLine = "Второй", LinePoints = GetLinePoints2() },
                        new LineData() { NameLine = "Третий", LinePoints = GetLinePoints3() }
                    }
                }))
            };

            clientReport.GenerateReport(actions);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            clientReport.OpenPreview();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            clientReport.SaveDocument();
        }

        #region Генерация данных для таблиц

        private TableModel GetTableData()
        {
            TableModel tableModel = new TableModel(new HeaderTable()
            {
                Headers = new List<string>() { "Profit", "DD", "Recovery", "Avg. Del", "Deal Count", "Symbol" }
            }, new TableSetting(),
            new List<List<string>>());

            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                tableModel.TableData.Add(new List<string>()
                {
                    Math.Round(random.NextDouble() * 1000.0, 4).ToString(),
                    Math.Round(random.NextDouble() * 1000.0, 4).ToString(),
                    Math.Round(random.NextDouble(), 2).ToString(),
                    Math.Round(random.NextDouble(), 4).ToString(),
                    random.Next(600, 5000).ToString(),
                    "ETHUSDT.txt",
                });
            }

            return tableModel;
        }
        private TableModel GetTableData2()
        {
            TableModel tableModel = new TableModel(new HeaderTable()
            {
                Headers = new List<string>() { "Time Type", "Time", "Stat Total", "Stat Norm", "Moving Average:1" }
            }, new TableSetting(),
            new List<List<string>>());

            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                tableModel.TableData.Add(new List<string>()
                {
                    "Minute",
                    "15",
                    Math.Round(random.NextDouble() * 3.0, 7).ToString(),
                    Math.Round(random.NextDouble(), 7).ToString(),
                    random.Next(5, 50).ToString()
                });
            }

            return tableModel;
        }
        private TableModel GetTableData3()
        {
            TableModel tableModel = new TableModel(new HeaderTable()
            {
                Headers = new List<string>() { "Moving Averagre:2", "Param. Total", "Param. Morm" }
            }, new TableSetting(),
            new List<List<string>>());

            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                tableModel.TableData.Add(new List<string>()
                {
                    random.Next(5, 50).ToString(),
                    Math.Round(random.NextDouble(), 7).ToString(),
                    random.NextDouble().ToString(),
                });
            }

            return tableModel;
        }

        #endregion

        #region Генерация данных для гистограмм

        public IEnumerable<HistrogramData> GetDataHistogram()
        {
            // X -> Param Norm
            // Y -> StatNorm

            List<HistrogramData> histrogramDatas = new List<HistrogramData>();

            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                histrogramDatas.Add(new HistrogramData(random.Next(1, 500), random.Next(1, 500)));
            }

            return histrogramDatas;
        }

        #endregion

        #region Генерация данных для Pie charts

        public IEnumerable<PieData> GetPieData()
        {
            return new List<PieData>()
            {
                new PieData() { Argument = "BTC", Value = 10 },
                new PieData() { Argument = "ETH", Value = 36 },
                new PieData() { Argument = "Doge", Value = 50 },
                new PieData() { Argument = "EOS", Value = 61 },
                new PieData() { Argument = "AVAX", Value = 89 },
            };
        }

        #endregion

        #region Генерация данных для Area

        public IEnumerable<AreaPoint> GetAreaPoints()
        {
            return new List<AreaPoint>()
            {
                new AreaPoint() { XValue = 1, YValue = 15 },
                new AreaPoint() { XValue = 2, YValue = 18 },
                new AreaPoint() { XValue = 3, YValue = 25 },
                new AreaPoint() { XValue = 4, YValue = 33 },
                new AreaPoint() { XValue = 5, YValue = 39 },
            };
        }

        public IEnumerable<AreaPoint> GetAreaPoints2()
        {
            return new List<AreaPoint>()
            {
                new AreaPoint() { XValue = 1, YValue = 10 },
                new AreaPoint() { XValue = 2, YValue = 12 },
                new AreaPoint() { XValue = 3, YValue = 14 },
                new AreaPoint() { XValue = 4, YValue = 17 },
                new AreaPoint() { XValue = 5, YValue = 21 },
            };
        }

        public IEnumerable<AreaPoint> GetAreaPoints3()
        {
            return new List<AreaPoint>()
            {
                new AreaPoint() { XValue = 2, YValue = 10 },
                new AreaPoint() { XValue = 3, YValue = 12 },
                new AreaPoint() { XValue = 1, YValue = 14 },
                new AreaPoint() { XValue = 5, YValue = 17 },
                new AreaPoint() { XValue = 4, YValue = 21 },
            };
        }

        #endregion

        #region Генерация данных для Line

        public IEnumerable<LinePoint> GetLinePoints()
        {
            return new List<LinePoint>()
            {
                new LinePoint() { XValue = 2, YValue = 10 },
                new LinePoint() { XValue = 3, YValue = 12 },
                new LinePoint() { XValue = 1, YValue = 14 },
                new LinePoint() { XValue = 5, YValue = 17 },
                new LinePoint() { XValue = 4, YValue = 21 },
            };
        }

        public IEnumerable<LinePoint> GetLinePoints2()
        {
            return new List<LinePoint>()
            {
                new LinePoint() { XValue = 1, YValue = 10 },
                new LinePoint() { XValue = 2, YValue = 12 },
                new LinePoint() { XValue = 3, YValue = 14 },
                new LinePoint() { XValue = 4, YValue = 17 },
                new LinePoint() { XValue = 5, YValue = 21 },
            };
        }

        public IEnumerable<LinePoint> GetLinePoints3()
        {
            return new List<LinePoint>()
            {
                new LinePoint() { XValue = 1, YValue = 15 },
                new LinePoint() { XValue = 2, YValue = 18 },
                new LinePoint() { XValue = 3, YValue = 25 },
                new LinePoint() { XValue = 4, YValue = 33 },
                new LinePoint() { XValue = 5, YValue = 39 },
            };
        }

        #endregion
    }
}
