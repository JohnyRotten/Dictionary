using System.Linq;
using NUnit.Framework;

namespace Dictionary
{
    [TestFixture]
    public class RamDataProviderTest
    {
        [Test]
        public void AddItemsTest()
        {
            IDataProvider dp = new RamDataProvider();
            dp.AddItems(AddedItems);
            Assert.AreEqual(dp.Items.Length, 4);
        }

        [Test]
        public void FindItemsTest()
        {
            IDataProvider dp = new RamDataProvider();
            dp.AddItems(AddedItems);
            Assert.AreEqual(dp.Search("a").Length, 2);
        }

        private string[] AddedItems => new string[] { "Foo", "Bar", "Test", "Abbr" };
    }
}