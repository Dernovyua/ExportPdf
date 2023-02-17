using Export.Interfaces;
using Export.Models;
using System;
using System.Collections.Generic;

namespace Export
{
    public class ClientReport : IExport, IDisposable
    {
        private IReport _report { get; set; }

        #region constructors

        public ClientReport()
        {
        }

        public ClientReport(IReport report)
        {
            _report = report;
        }

        #endregion

        public void SetExport(IReport report)
        {
            _report = report;
        }

        /// <summary>
        /// Метод вызывается пользователем
        /// Один метод, в котором лежит последовательность вызова методов, заложенной пользователем
        /// </summary>
        /// <param name="actions">Методы</param>
        public void GenerateReport(IEnumerable<Action> actions)
        {
            _report.GetCallSequenceMethods(actions);
        }

        public void AddText(Text text)
        {
            _report.AddText(text);
        }

        public void AddChart(Chart chartData)
        {
            _report.AddChart(chartData);
        }

        public void AddTable(TableModel table)
        {
            _report.AddTable(table);
        }

        public void SaveDocument()
        {
            _report.SaveDocument();
        }

        public void OpenPreview()
        {
            _report.OpenPreview();
        }

        public void AddNewPage(string? name = null)
        {
            _report.AddNewPage(name);
        }

        public void Dispose()
        {
            
        }
    }
}
