using Lab1.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab1.Tests
{
    [TestClass]
    public class DynamicListTests
    {
        [TestMethod]
        public void TestAddItems()
        {
            var actualList = new DynamicList<int> { 1, 2, 3, 4, 5 };
            var expectedListAfterAdding = new DynamicList<int> { 1, 2, 3, 4, 5, 6, 8 };

            actualList.Add(6);
            actualList.Add(8);

            CollectionAssert.AreEqual(expectedListAfterAdding, actualList);
        }

        [TestMethod]
        public void TestRemoveItemsByIndex()
        {
            var actualList = new DynamicList<int> { 1, 2, 3, 4, 5, 6, 8 };
            var expectedListAfterRemovindByIndex = new DynamicList<int> { 3, 4, 5, 6, 8 };

            actualList.RemoveAt(0);
            actualList.RemoveAt(0);

            CollectionAssert.AreEqual(expectedListAfterRemovindByIndex, actualList);
        }

        [TestMethod]
        public void TestRemoveItemsByValue()
        {
            var actualList = new DynamicList<int> { 3, 4, 5, 6, 8 };
            var expectedListAfterRemovindByIndex = new DynamicList<int> { 3, 4, 5 };

            actualList.Remove(6);
            actualList.Remove(8);

            CollectionAssert.AreEqual(expectedListAfterRemovindByIndex, actualList);
        }

        [TestMethod]
        public void TestForeachOnItems()
        {
            var actualList = new DynamicList<int> { 3, 4, 5, 6, 8 };

            foreach (var item in actualList)
            {
                Assert.IsTrue(item != 0);
            }
        }

        [TestMethod]
        public void TestClearList()
        {
            var actualList = new DynamicList<int> { 3, 4, 5 };

            actualList.Clear();

            Assert.AreEqual(0, actualList.Count);
        }

        [TestMethod]
        public void TestItemByIndex()
        {
            var actualList = new DynamicList<int> { 1, 2, 3, 4, 5 };

            var item = actualList[2];

            Assert.AreEqual(3, item);
        }
    }
}