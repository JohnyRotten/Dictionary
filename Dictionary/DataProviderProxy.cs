using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dictionary
{
    class DataProviderProxy : IDataProvider

    {
        public IDataProvider DataProvider { get; set; }

        private HashSet<string> _words = new HashSet<string>(); 

        public DataProviderProxy(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
            DataProvider.UpdateListEvent += ParentListChanged;
            UpdateWords();
        }

        private void ParentListChanged()
        {
            UpdateWords();
            OnUpdateListEvent();
        }

        private void UpdateWords()
        {
            _words.Clear();
            foreach (var item in DataProvider.Items)
            {
                _words.Add(item);
            }
        }

        public event UpdateListHandler UpdateListEvent;

        public void AddItems(string[] items)
        {
            DataProvider.AddItems(items);
        }

        public string[] Items => _words.ToArray();

        public string[] Search(string exp)
        {
            var regex = new Regex(exp, RegexOptions.IgnoreCase);
            var query = from w in _words where regex.IsMatch(w) select w;
            return query.ToArray();
        }

        public void Clear()
        {
            DataProvider.Clear();
        }

        protected virtual void OnUpdateListEvent()
        {
            UpdateListEvent?.Invoke();
        }
    }
}
