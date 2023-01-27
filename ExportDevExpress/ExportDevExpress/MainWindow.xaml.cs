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
                new PieData() { Argument = "Doge", Value = 21000 },
                new PieData() { Argument = "EOS", Value = 863 },
                new PieData() { Argument = "AVAX", Value = 89 },
            };
        }

        #endregion
    }
}
