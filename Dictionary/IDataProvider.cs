using System.Security.RightsManagement;

namespace Dictionary
{
    public delegate void UpdateListHandler();

    public interface IDataProvider
    {
        event UpdateListHandler UpdateListEvent;

        void AddItems(string[] items);

        string[] Items { get; }

        string[] Search(string exp);

        void Clear();
    }
}