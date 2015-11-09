using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dictionary
{
    public class RamDataProvider : IDataProvider
    {
        private HashSet<string> _words = new HashSet<string>(); 

        public event UpdateListHandler UpdateListEvent;

        public void AddItems(string[] items)
        {
            foreach (var item in items)
            {
                _words.Add(item);
            }
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
            _words.Clear();
        }
    }
}