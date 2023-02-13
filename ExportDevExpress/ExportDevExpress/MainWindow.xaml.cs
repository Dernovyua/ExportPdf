using Export;
using Export.Enums;
using Export.Models;
using Export.Models.Charts;
using Export.ModelsExport;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

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
            clientReport.SetExport(new Pdf(@"C:\Users\Flax\Desktop", "chartTest"));

            clientReport.GenerateReport(new List<Action>()
            {
                () => clientReport.AddChart(new Chart(new Histogram(new List<double> () { 5, 7, 9, 10, 14 }, new SettingChart())))
            });
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
            }, new TableSetting() { SettingText = new SettingText() { Color = Color.Red } },
            new List<List<object>>());

            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {
                tableModel.TableData.Add(new List<object>()
                {
                    Math.Round(random.NextDouble() * 1000.0, 4),
                    Math.Round(random.NextDouble() * 1000.0, 4),
                    Math.Round(random.NextDouble(), 2),
                    Math.Round(random.NextDouble(), 4),
                    random.Next(600, 5000),
                    "Привет.txt",
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
            new List<List<object>>());

            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                tableModel.TableData.Add(new List<object>()
                {
                    "Minute",
                    "15",
                    Math.Round(random.NextDouble() * 3.0, 7),
                    Math.Round(random.NextDouble(), 7),
                    random.Next(5, 50)
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
            new List<List<object>>());

            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                tableModel.TableData.Add(new List<object>()
                {
                    random.Next(5, 50),
                    Math.Round(random.NextDouble(), 7),
                    random.NextDouble(),
                });
            }

            return tableModel;
        }

        #endregion


        #region Генерация данных для Pie charts

        public IEnumerable<DoughnutData> GetDoughnutData()
        {
            return new List<DoughnutData>()
            {
                new DoughnutData() { Argument = "BTC", Value = 10 },
                new DoughnutData() { Argument = "ETH", Value = 36 },
                new DoughnutData() { Argument = "Doge", Value = 50 },
                new DoughnutData() { Argument = "EOS", Value = 61 },
                new DoughnutData() { Argument = "AVAX", Value = 89 },
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
