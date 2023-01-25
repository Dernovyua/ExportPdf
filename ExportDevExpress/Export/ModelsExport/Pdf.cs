using Export.Interfaces;
using Export.Models;
using System;
using System.Collections.Generic;

namespace Export.ModelsExport
{
    public class Pdf : ISequence
    {
        // Здесь будут еще свойства от DevExpress

        public void AddChart(Chart chartData)
        {
            // Логика формирования графика
        }

        public void AddTable(Table table)
        {
            // Логика формирования/заполнения таблицы
        }

        public void AddText(Text text)
        {
            // Логика добавления текста
        }

        /// <summary>
        /// Вызов методов в нужной последовательности (который заложит пользователь при добавлении в список)
        /// </summary>
        /// <param name="actions"></param>
        public void GetCallSequenceMethods(IEnumerable<Action> actions)
        {
            foreach (var action in actions)
            {
                action();
            }
        }
    }
}
