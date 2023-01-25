using Export.Interfaces;
using Export.Models;
using System;
using System.Collections.Generic;

namespace Export
{
    public class ClientReport : IMethodAdding
    {
        private ISequence _sequence { get; set; }

        #region constructors

        public ClientReport() { }

        public ClientReport(ISequence sequence)
        {
            _sequence = sequence;
        }

        #endregion

        public void SetExport(ISequence sequence)
        {
            _sequence = sequence;
        }

        /// <summary>
        /// ����� ���������� �������������
        /// ���� �����, � ������� ����� ������������������ ������ �������, ���������� �������������
        /// </summary>
        /// <param name="actions">������</param>
        public void GenerateReport(IEnumerable<Action> actions)
        {
            _sequence.GetCallSequenceMethods(actions);
        }

        public void AddText(Text text)
        {
            _sequence.AddText(text);
        }

        public void AddChart(Chart chartData)
        {
            _sequence.AddChart(chartData);
        }

        public void AddTable(Table table)
        {
            _sequence.AddTable(table);
        }
    }
}
