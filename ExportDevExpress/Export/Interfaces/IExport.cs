using Export.Models;

namespace Export.Interfaces
{
    public interface IExport
    {
        /// <summary>
        /// Добавление текста в файл экспорта
        /// </summary>
        /// <param name="text">Текст для добавления</param>
        void AddText(Text text);

        /// <summary>
        /// Добавления графика в файл экспорта
        /// </summary>
        /// <param name="chart">График</param>
        void AddChart(Chart chart);

        /// <summary>
        /// Добавление таблицы в файл экспорта
        /// </summary>
        /// <param name="table">Таблица (ее данные и настройка)</param>
        void AddTable(TableModel table);

        /// <summary>
        /// Добавление новой страницы в файле экспорта
        /// </summary>
        void AddNewPage(string? name = null);

        /// <summary>
        /// Сохранение документа
        /// </summary>
        void SaveDocument();

        /// <summary>
        /// Открытие превью файла
        /// </summary>
        void OpenPreview();
    }
}
