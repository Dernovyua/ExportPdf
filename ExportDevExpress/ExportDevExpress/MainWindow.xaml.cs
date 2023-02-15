using Export;
using Export.Models;
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
            //clientReport.SetExport(new Pdf(@"C:\Users\Flax\Desktop", "chartTest"));

            //clientReport.GenerateReport(new List<Action>()
            //{
            //    () => clientReport.AddText(new Text("Открыть файл file.xlsx", new SettingText(), new HyperLink()
            //    {
            //        LinkText = "file.xlsx",

            //        TargetLink = @"C:\Users\Flax\Desktop\file.xlsx",
            //    })),
            //    () => clientReport.AddText(new Text("gfoooooooooooooooooo"))
            //});

            clientReport.SetExport(new Excel(@"C:\Users\Flax\Desktop", "chartTestExcel"));
            clientReport.GenerateReport(new List<Action>()
            {
                () => clientReport.AddTable(GetTableData()),
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
    }
}
