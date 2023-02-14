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

        public ClientReport(IReport sequence)
        {
            _report = sequence;
        }

        #endregion

        public void SetExport(IReport sequence)
        {
            _report = sequence;
        }

        /// <summary>
        /// ����� ���������� �������������
        /// ���� �����, � ������� ����� ������������������ ������ �������, ���������� �������������
        /// </summary>
        /// <param name="actions">������</param>
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

        public void AddNewPage()
        {
            _report.AddNewPage();
        }

        public void Dispose()
        {
            
        }
    }
}
