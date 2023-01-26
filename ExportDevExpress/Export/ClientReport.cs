using Export.Interfaces;
using Export.Models;
using System;
using System.Collections.Generic;

namespace Export
{
    public class ClientReport : IExport
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
        /// Метод вызывается пользователем
        /// Один метод, в котором лежит последовательность вызова методов, заложенной пользователем
        /// </summary>
        /// <param name="actions">Методы</param>
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

        public void AddTable(TableModel table)
        {
            _sequence.AddTable(table);
        }

        public void SaveDocument()
        {
            _sequence.SaveDocument();
        }

        public void OpenPreview()
        {
            _sequence.OpenPreview();
        }
    }
}
