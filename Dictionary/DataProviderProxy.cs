using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dictionary
{
    class DataProviderProxy : IDataProvider

    {
        private IDataProvider _dataProvider;
        private HashSet<string> _words = new HashSet<string>(); 

        public DataProviderProxy(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _dataProvider.UpdateListEvent += OnUpdateListEvent;
            _dataProvider.UpdateListEvent += UpdateWords;
            UpdateWords();
        }

        private void UpdateWords()
        {
            _words.Clear();
            foreach (var item in _dataProvider.Items)
            {
                _words.Add(item);
            }
        }

        public event UpdateListHandler UpdateListEvent;

        public void AddItems(string[] items)
        {
            _dataProvider.AddItems(items);
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
            _dataProvider.Clear();
        }

        protected virtual void OnUpdateListEvent()
        {
            UpdateListEvent?.Invoke();
        }
    }
}
