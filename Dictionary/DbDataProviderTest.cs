using NUnit.Framework;

namespace Dictionary
{
    [TestFixture]
    public class DbDataProviderTest
    {
        [Test]
        public void AddItemsTest()
        {
            IDataProvider dp = new DbDataProvider();
            dp.Clear();
            dp.AddItems(AddedItems);
            Assert.AreEqual(4, dp.Items.Length);
            dp.Clear();
        }

        [Test]
        public void ClearTest()
        {
            IDataProvider dp = new DbDataProvider();
            dp.Clear();
            dp.AddItems(AddedItems);
            Assert.AreEqual(AddedItems.Length, dp.Items.Length);
            dp.Clear();
            Assert.AreEqual(0, dp.Items.Length);
        }

        [Test]
        public void FindItemsTest()
        {
            IDataProvider dp = new DbDataProvider();
            dp.Clear();
            dp.AddItems(AddedItems);
            Assert.AreEqual(2, dp.Search("a").Length);
            dp.Clear();
        }

        [Test]
        public void AddNewItemTest()
        {
            IDataProvider dp = new DbDataProvider();
            dp.Clear();
            dp.AddItems(AddedItems);
            int countBefore = dp.Items.Length;
            var newItems = new string[] { "AAA", "BBB" };
            dp.AddItems(newItems);
            int countAfter = dp.Items.Length;
            Assert.AreEqual(newItems.Length, countAfter - countBefore);
            dp.Clear();
        }

        [Test]
        public void AddContaintsItemTest()
        {
            IDataProvider dp = new DbDataProvider();
            dp.Clear();
            dp.AddItems(AddedItems);
            int countBefore = dp.Items.Length;
            dp.AddItems(AddedItems);
            int countAfter = dp.Items.Length;
            Assert.AreEqual(0, countAfter - countBefore);
            dp.Clear();
        }

        private string[] AddedItems => new string[] { "Foo", "Bar", "Test", "Abbr" };
    }
}