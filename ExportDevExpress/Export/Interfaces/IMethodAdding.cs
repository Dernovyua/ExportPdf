using Export.Models;

namespace Export.Interfaces
{
    public interface IMethodAdding
    {
        void AddText(Text text);
        void AddChart(Chart chartData);
        void AddTable(Table table);
    }
}
