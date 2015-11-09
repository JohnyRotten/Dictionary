using System;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

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

        public void AddItems(string[] items)
        {
            var db = new DataContext(ConnectionString);
            var words = db.GetTable<Word>();
            foreach (var item in items)
            {
                try
                {
                    words.InsertOnSubmit(new Word { Data = item });
                    db.SubmitChanges();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public string[] Items
        {
            get
            {
                var words = new DataContext(ConnectionString).GetTable<Word>();
                var query = from w in words select w.Data;
                return query.ToArray();
            }
        }

        public string[] Search(string exp)
        {
            var words = new DataContext(ConnectionString).GetTable<Word>();
            var query = from w in words where w.Data.Contains(exp) select w.Data;
            return query.ToArray();
        }

        public void Clear()
        {
            var db = new DataContext(ConnectionString);
            var words = db.GetTable<Word>();
            words.DeleteAllOnSubmit(words);
            db.SubmitChanges();
        }

        private string ConnectionString => ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}