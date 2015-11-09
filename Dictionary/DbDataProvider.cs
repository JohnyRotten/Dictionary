using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Dictionary
{
    [Table(Name = "Dictionary")]
    class Word
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column(CanBeNull = false, DbType = "nvarchar(20)")]
        public string Data { get; set; }
    }

    public class DbDataProvider : IDataProvider
    {
        public event UpdateListHandler UpdateListEvent;
        private string _connectionString;

        public DbDataProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Table<Word> Words => new DataContext(_connectionString).GetTable<Word>();

        public void AddItems(string[] items)
        {
            var words = Words;
            foreach (var item in items)
            {
                try
                {
                    words.InsertOnSubmit(new Word { Data = item });
                    words.Context.SubmitChanges();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            OnUpdateListEvent();
        }

        public string[] Items
        {
            get
            {
                var query = from w in Words select w.Data;
                return query.ToArray();
            }
        }

        public string[] Search(string exp)
        {
            var query = from w in Words where w.Data.Contains(exp) select w.Data;
            return query.ToArray();
        }

        public void Clear()
        {
            var words = Words;
            words.DeleteAllOnSubmit(words);
            words.Context.SubmitChanges();
            OnUpdateListEvent();
        }

        protected virtual void OnUpdateListEvent()
        {
            UpdateListEvent?.Invoke();
        }
    }
}