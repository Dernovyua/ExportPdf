using Export;
using Export.Enums;
using Export.Models;
using Export.ModelsExport;
using System;
using System.Collections.Generic;
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
            clientReport.SetExport(new Pdf());

            List<Action> actions = new List<Action>()
            {
                () => clientReport.AddTable(new Table(null, null)),
                () => clientReport.AddText(new Text("", new SettingText())),
                () => clientReport.AddChart(new Chart(new SettingChart(1, 1, "", "", TypeChart.Pie), new ChartData())),
            };

            clientReport.GenerateReport(actions);
        }
    }
}
