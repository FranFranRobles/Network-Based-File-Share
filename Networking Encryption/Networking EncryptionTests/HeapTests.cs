using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class HeapTests
    {
        const string PROVIDER_TYPE = "Microsoft.VisualStudio.TestTools.DataSource.XML";
        const string FILE = "|DataDirectory|\\Heap.xml";
        const string HEAP_CAT = "Heap Tests";
        const string EXCEPTION_CAT = "Exception";
        const string TEST_CONST = "insertRemove";
        const string REPLACE_TEST = "replace";
        const string REPLACE_NUM = "num";
        const int MINVAL = 0;
        const int MAXVAL = 1000000;
        const string SIZE = "size";

        #region Test intializer
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        Heap<int> heap = null;
        [TestInitialize]
        public void TestIntializer()
        {
            heap = new Heap<int>(compareInts);
        }
        #endregion

        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        public void TestDefaultCTOR()
        {
            Assert.IsTrue(heap.IsEmpty, "heap should be empty");
            Assert.AreEqual(0, heap.Size, "heap should be empty");
        }
        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, TEST_CONST, DataAccessMethod.Sequential)]
        public void TestHeapafyArr()
        {
            int size = Convert.ToInt32(TestContext.DataRow[SIZE]);
            int[] list = intializeArr(size);
            heap = new Heap<int>(compareInts, list);
            Assert.IsFalse(heap.IsEmpty, "heap should not be empty");
            Assert.AreEqual(size, heap.Size, "heap should not be empty");
            Array.Sort(list);
            TestsRemove(list);
            Assert.IsTrue(heap.IsEmpty, "heap should be empty");
            Assert.AreEqual(0, heap.Size, "heap should be empty");
        }
        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, TEST_CONST, DataAccessMethod.Sequential)]
        public void TestMergeHeap()
        {
            int size = Convert.ToInt32(TestContext.DataRow[SIZE]);
            int[] list = intializeArr(size);
            Heap<int> mergeHeap = new Heap<int>(compareInts, list);
            heap = new Heap<int>(mergeHeap);
            Assert.IsFalse(mergeHeap.IsEmpty, "heap should not be empty");
            Assert.AreEqual(size, mergeHeap.Size, "heap should not be empty");
            Assert.IsFalse(heap.IsEmpty, "heap should not be empty");
            Assert.AreEqual(mergeHeap.Size, heap.Size, "heap should not be empty");
            while (heap.Size > 0)
            {
                Assert.AreEqual(mergeHeap.Remove(), heap.Remove(), "removed different elements");
            }
            Assert.AreEqual(mergeHeap.Size, heap.Size, "invalid size found");
            Assert.AreEqual(0, heap.Size, "heap should be empty");
            Assert.AreEqual(mergeHeap.IsEmpty, heap.IsEmpty, "expected a different value");
            Assert.IsTrue(heap.IsEmpty, "heap should be empty");
        }
        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, TEST_CONST, DataAccessMethod.Sequential)]
        public void TestAdditionOpOverload()
        {
            int size = Convert.ToInt32(TestContext.DataRow[SIZE]);
            int[] list = intializeArr(size);
            Heap<int> heapTwo = new Heap<int>(compareInts,list);
            heap = new Heap<int>(heapTwo);
            Assert.IsFalse(heap.IsEmpty, "should not be empty");
            Assert.IsFalse(heapTwo.IsEmpty, "should not be empty");
            Heap<int> HeapThree = heap + heapTwo;
            Assert.IsFalse(HeapThree.IsEmpty, "should not be empty");
            Assert.IsFalse(heap.IsEmpty, "should not be empty");
            Assert.IsFalse(heapTwo.IsEmpty, "should not be empty");
            Assert.AreEqual(heap.Size + heapTwo.Size, HeapThree.Size, "Invalid Size Returned");
            Assert.AreEqual(2 * size, HeapThree.Size, "heap should not be empty");
            Array.Sort(list);
            int index = 0;
            while (HeapThree.Size > 0)
            {
                Assert.AreEqual(list[index], HeapThree.Remove(), "removed different elements");
                Assert.AreEqual(list[index++], HeapThree.Remove(), "removed different elements");
            }
            Assert.AreEqual(heap.Size, heapTwo.Size, "Sizes should be Equal");
            Assert.AreEqual(heap.IsEmpty, heapTwo.IsEmpty, "heaps should contain the same states");
            Assert.IsFalse(heap.IsEmpty);
            Assert.AreNotEqual(0, heap.Size);
            Assert.AreEqual(0, HeapThree.Size, "heap should be empty");
            Assert.IsTrue(HeapThree.IsEmpty, "heap should be empty");
        }
        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, TEST_CONST, DataAccessMethod.Sequential)]
        public void InsertTest()
        {
            Assert.AreEqual(0, heap.Size, "heap should be empty");
            int size = Convert.ToInt32(TestContext.DataRow[SIZE]);
            int[] list = intializeArr(size);
            Assert.IsTrue(heap.IsEmpty, "heap should be empty");
            for (int index = 0; index < list.Length; index++)
            {
                heap.Insert(list[index]);
            }
            Assert.IsFalse(heap.IsEmpty, "heap should not be empty");
            Assert.AreEqual(size, heap.Size, "heap should not be empty");
            Array.Sort(list);
            TestsRemove(list);
            Assert.IsTrue(heap.IsEmpty);
            Assert.AreEqual(0, heap.Size, "heap should be empty");
        }

        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, TEST_CONST, DataAccessMethod.Sequential)]
        public void RemoveTest()
        {
            int size = Convert.ToInt32(TestContext.DataRow[SIZE]);
            int[] list = intializeArr(size);
            heap = new Heap<int>(compareInts, list);
            Assert.IsFalse(heap.IsEmpty, "heap should not be empty");
            Assert.AreEqual(size, heap.Size, "heap should not be empty");
            Array.Sort(list);
            TestsRemove(list);
            Assert.IsTrue(heap.IsEmpty, "heap should be empty");
            Assert.AreEqual(0, heap.Size, "heap should be empty");
        }
        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, TEST_CONST, DataAccessMethod.Sequential)]
        public void ReplaceTest()
        {
            Assert.AreEqual(0, heap.Size, "heap should be empty");
            int size = Convert.ToInt32(TestContext.DataRow[SIZE]);
            int[] list = intializeArr(size);
            List<int> intializer = new List<int>();
            List<int> ReplaceList = new List<int>();
            for (int index = 0; index < list.Length; index++)
            {
                if (index % 2 == 0)
                {
                    intializer.Add(list[index]);    
                }
                else
                {
                    ReplaceList.Add(list[index]);
                }
            }
            heap = new Heap<int>(compareInts, intializer.ToArray());
            Assert.IsFalse(heap.IsEmpty, " heap should not be empty");
            Assert.AreEqual(intializer.Count, heap.Size, " invailid heap size");
            int i = 0;
            for (int index = 0; index < ReplaceList.Count; index++)
            {
                Assert.AreEqual(list[i++], heap.Replace(ReplaceList[index]));
            }
            Assert.IsFalse(heap.IsEmpty, "heap should not be empty");
            Assert.AreEqual(list.Length - i, heap.Size, " invalid length found");
            while (heap.Size > 0)
            {
                Assert.AreEqual(list[i++], heap.Remove());
            }
            Assert.IsTrue(heap.IsEmpty, "heap should be empty");
            Assert.AreEqual(0, heap.Size, " heap should be empty");
        }
        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, TEST_CONST, DataAccessMethod.Sequential)]
        public void FindMinTest()
        {
            Assert.AreEqual(0, heap.Size, "heap should be empty");
            int size = Convert.ToInt32(TestContext.DataRow[SIZE]);
            int[] list = intializeArr(size);
            Assert.IsTrue(heap.IsEmpty, "heap should be empty");
            for (int index = 0; index < list.Length; index++)
            {
                heap.Insert(list[index]);
            }
            Assert.IsFalse(heap.IsEmpty, "heap should not be empty");
            Assert.AreEqual(size, heap.Size, "heap should not be empty");
            Array.Sort(list);
            for (int index = 0; index < list.Length; index++)
            {
                Assert.AreEqual(list[index], heap.FindMin(), "incorrect min returned");
                Assert.AreEqual(list[index], heap.Remove(), "returned the wrong element");
            }
            Assert.IsTrue(heap.IsEmpty, "heap should be empty");
            Assert.AreEqual(0, heap.Size, "heap should be empty");
        }
        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [TestCategory(EXCEPTION_CAT)]
        [ExpectedException(typeof(InvalidOperationException))]
        [DataSource(PROVIDER_TYPE, FILE, REPLACE_TEST, DataAccessMethod.Sequential)]
        public void HeapReplaceSizeZero()
        {
            Assert.AreEqual(0, heap.Size, "heap should be empty");
            Assert.IsTrue(heap.IsEmpty, " heap should be empty");
            int insertNum = Convert.ToInt32(TestContext.DataRow[REPLACE_NUM]);
            heap.Replace(insertNum);
        }
        [TestMethod()]
        [TestCategory(HEAP_CAT)]
        [TestCategory(EXCEPTION_CAT)]
        [ExpectedException(typeof(InvalidOperationException))]
        [DataSource(PROVIDER_TYPE, FILE, REPLACE_TEST, DataAccessMethod.Sequential)]
        public void HeapReplaceSizeOne()
        {
            Assert.AreEqual(0, heap.Size, "heap should be empty");
            Assert.IsTrue(heap.IsEmpty, " heap should be empty");
            int insertNum = Convert.ToInt32(TestContext.DataRow[REPLACE_NUM]);
            heap.Insert(1);
            Assert.IsFalse(heap.IsEmpty, " heap should not be empty");
            Assert.AreEqual(1, heap.Size, "heap should contain one element");
            heap.Replace(insertNum);
        }
        #region Test Helpers
        /// <summary>
        /// function compares the following equality left < right
        /// <para>Returns true if left param is less than the right param</para>
        /// </summary>
        /// <param name="left">left value</param>
        /// <param name="right">right vale</param>
        /// <returns></returns>
        static bool compareInts(int left, int right)
        {
            return left < right ? true : false;
        }
        /// <summary>
        /// creates an array to the specified size
        /// <para>an array filled with random nubmers</para>
        /// </summary>
        /// <param name="size">size of the array to create</param>
        /// <returns> an array filled with random numbers</returns>
        static int[] intializeArr(int size)
        {
            int[] temp = new int[size];
            for (int i = 0; i < size; i++)
            {
                temp[i] = i + 1;
            }
            //Array.Sort(temp,);
            return temp;
        }
        /// <summary>
        /// test to check if the given array is found inside the heap
        /// </summary>
        /// <param name="list">list to check</param>
        private void TestsRemove(int[] list)
        {
            foreach (int index in list)
            {
                Assert.AreEqual(index, heap.Remove());
            }
        }
        #endregion

    }
}