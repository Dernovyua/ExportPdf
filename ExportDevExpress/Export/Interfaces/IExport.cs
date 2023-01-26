﻿using Export.Models;

namespace Export.Interfaces
{
    public interface IExport
    {
        void AddText(Text text);
        void AddChart(Chart chart);
        void AddTable(TableModel table);
        void SaveDocument();
        void OpenPreview();
    }
}