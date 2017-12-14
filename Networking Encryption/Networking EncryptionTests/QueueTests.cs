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
    public class QueueTests
    {
        //test Constants
        const string SIZE = "Size";
        const string CAPACITY = "Capacity";
        const string EXCEPTION = "Exception";
        const string CATEGORY = "Queue Tests";
        const string TEST_NODE = "CTOR_Settings";
        const string TEST_FILE = "|DataDirectory|\\Queue.xml";
        const string PROVIDER_TYPE = "Microsoft.VisualStudio.TestTools.DataSource.XML";

        #region Test Intializer
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        Queue<int> queue = null;
        [TestInitialize]
        public void TestIntializer()
        {
            queue = new Queue<int>();
        }
        #endregion

        #region CTOR Tests
        [TestMethod()]
        [TestCategory(CATEGORY)]
        public void DefaultCTOR_Test()
        {
            Assert.AreEqual(0, queue.Size, "Invalid Default Size Found");
            Assert.AreEqual(0, queue.Capacity, "Invalid Default Size Found");
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void CapacityCTORTest()
        {
            uint capacity = Convert.ToUInt32(TestContext.DataRow[CAPACITY]);
            queue = new Queue<int>(capacity);
            Assert.AreEqual(0, queue.Size, "Invalid Size Found");
            Assert.AreEqual(capacity, queue.Capacity, "Invalid Size Found");
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void arrayCTORTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            int[] intializerList = listGen(size);
            queue = new Queue<int>(intializerList);
            Assert.AreEqual(size, queue.Size, "Invalid Size Found");
            Assert.IsTrue(queue.Capacity >= size, "invalid Capacity found");
            TestContainment(intializerList, queue);
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void CopyCTORTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            int[] intializerList = listGen(size);
            queue = new Queue<int>(intializerList);
            Assert.AreEqual(size, queue.Size, "Invalid Size Found");
            Assert.IsTrue(queue.Capacity >= size, "invalid Capacity found");
            TestContainment(intializerList, queue);
            Queue<int> testQueue = new Queue<int>(queue);
            Assert.AreEqual(queue.Size, testQueue.Size, "Copied queue is not of same length as original queue");
            Assert.AreEqual(queue.Capacity, testQueue.Capacity, "Copied queue is not of same length as orginal");
            TestContainment(intializerList, testQueue);
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void CapWithCpyCTORTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            uint cap = Convert.ToUInt32(TestContext.DataRow[CAPACITY]);
            int[] intializerList = listGen(size);
            queue = new Queue<int>(intializerList);
            Assert.AreEqual(size, queue.Size, "Invalid Size Found");
            Assert.IsTrue(queue.Capacity >= size, "invalid Capacity found");
            TestContainment(intializerList, queue);
            Queue<int> testQueue = new Queue<int>(cap,queue);
            Assert.AreEqual(queue.Size, testQueue.Size, "Copied queue is not of same length as original queue");
            Assert.IsTrue(cap <= testQueue.Capacity, "que was intialize to invalid capcacity lenght");
            Assert.IsTrue(queue.Capacity <= testQueue.Capacity, "Copied queue is not of same length as orginal");
            TestContainment(intializerList, testQueue);
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void CapWithArrayCTORTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            uint cap = Convert.ToUInt32(TestContext.DataRow[CAPACITY]);
            int[] intializerList = listGen(size);
            queue = new Queue<int>(cap, intializerList);
            Assert.AreEqual(size, queue.Size, "Invalid Size Found");
            Assert.IsTrue(cap <= queue.Capacity, "Invalid capacity found");
            Assert.IsTrue(queue.Capacity >= size, "invalid Capacity found");
            TestContainment(intializerList, queue);
        }
        #endregion

        #region Enque & Deque Tests
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void EnqueTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            int[] intlist = listGen(size);
            foreach (int num in intlist)
            {
                queue.Enque(num);
            }
            Assert.AreEqual(size, queue.Size, "invalid size found");
            Assert.IsTrue(queue.Capacity >= queue.Size, " invalid queue capacity found");
            TestContainment(intlist, queue);
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void DequeNotEmptyTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            int[] intlist = listGen(size);
            foreach (int num in intlist)
            {
                queue.Enque(num);
            }
            Assert.AreEqual(size, queue.Size, "invalid size found");
            Assert.IsTrue(queue.Capacity >= queue.Size, " invalid queue capacity found");
            TestContainment(intlist, queue);
            foreach (int num in intlist)
            {
                Assert.AreEqual(num, queue.Deque(), "invalid deque value returned");
            }
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [TestCategory(EXCEPTION)]
        [ExpectedException(typeof(Exception))]
        public void DequeEmptyTest()
        {
            Assert.AreEqual(0, queue.Size, "invalid size found");
            Assert.AreEqual(0, queue.Capacity, "invalid capcity found");
            queue.Deque();
        }
        #endregion

        [TestMethod()]
        [TestCategory(CATEGORY)]
        public void ClearEmptyTest()
        {
            Assert.AreEqual(0, queue.Size, "invalid size found");
            Assert.AreEqual(0, queue.Capacity, "invalid capacity found");
            queue.Clear();
            Assert.AreEqual(0, queue.Size, "invalid size found");
            Assert.AreEqual(0, queue.Capacity, "invalid capacity found");
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void ClearNotEmptyTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            int[] intList = listGen(size);
            queue = new Queue<int>(intList);
            Assert.AreEqual(size, queue.Size, "invalid size found");
            Assert.AreNotEqual(0, queue.Capacity, "Capacity should not be zero");
            TestContainment(intList, queue);
            queue.Clear();
            foreach (int num in intList)
            {
                Assert.IsFalse(queue.Contains(num), "No values should be present");
            }
            Assert.AreEqual(0, queue.Size, " queue should be empty");
            Assert.AreEqual(0, queue.Capacity, "invalid queue capacity found");
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        public void ContainsEmptyTest()
        {
            const int TEST_VAL = 1;
            Assert.AreEqual(0, queue.Size, "invalid size found");
            Assert.AreEqual(0, queue.Capacity, "invalid queue capacity found");
            Assert.IsFalse(queue.Contains(TEST_VAL), "value should not be found");
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void ContainsNotEmptyTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            int[] intList = listGen(size);
            int NotFoundVal = -1;
            queue = new Queue<int>(intList);
            Assert.AreEqual(size, queue.Size, "invalid size found");
            Assert.AreNotEqual(0, queue.Capacity, "queu capcity should not be zero");
            TestContainment(intList, queue);
            foreach (int num in intList)
            {
                Assert.IsTrue(queue.Contains(num), "value should be found");
            }
            Assert.IsFalse(queue.Contains(NotFoundVal), "given value should not be found");
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [TestCategory(EXCEPTION)]
        [ExpectedException(typeof(Exception))]
        public void PeekEmptyTest()
        {
            queue.Peek();
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void PeekNotEmptyTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            int[] intList = listGen(size);
            queue = new Queue<int>(intList);
            Assert.AreEqual(size, queue.Size, "invalid size found");
            Assert.AreNotEqual(0, queue.Capacity, "queu capcity should not be zero");
            TestContainment(intList, queue);
            foreach (int num in intList)
            {
                Assert.IsTrue(queue.Contains(num), "value should be found");
                Assert.AreEqual(num, queue.Peek(), "incorrect value found");
                Assert.AreEqual(num, queue.Deque(), "incorrect value removed");
            }
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        public void ToArrayEmptyTest()
        {
            Assert.AreEqual(0, queue.Size, "invalid size found");
            Assert.AreEqual(0, queue.Capacity, "invalid queue capacity found");
            Assert.IsNull(queue.ToArray(), " array is not null");
        }
        [TestMethod()]
        [TestCategory(CATEGORY)]
        [DataSource(PROVIDER_TYPE, TEST_FILE, TEST_NODE, DataAccessMethod.Sequential)]
        public void ToArrayNotEmptyTest()
        {
            uint size = Convert.ToUInt32(TestContext.DataRow[SIZE]);
            int[] intList = listGen(size);
            queue = new Queue<int>(intList);
            Assert.AreEqual(size, queue.Size, "invalid size found");
            Assert.AreNotEqual(0, queue.Capacity, "queu capcity should not be zero");
            foreach (int num in intList)
            {
                Assert.IsTrue(queue.Contains(num), "value should be found");
                Assert.AreEqual(num, queue.Peek(), "incorrect value found");
                Assert.AreEqual(num, queue.Deque(), "incorrect value removed");
            }
        }

        #region Test Helper Functions
        /// <summary>
        /// function fills an array with random numbers
        /// <para>Returns an array with randomly filled numbers</para>
        /// </summary>
        /// <param name="size">holds the size of the array to be intialized</param>
        /// <returns>Returns an array with randomly filled numbers</returns>
        static int[] listGen(uint size)
        {
            int[] list = new int[size];
            Random ranGen = new Random();
            for (int index = 0; index < size; index++)
            {
                list[index] = ranGen.Next();
            }
            return list;
        }
        /// <summary>
        /// function Tests whether the values of the array are found within
        /// the queue
        /// </summary>
        /// <param name="list">is the array that holds values to test for containment</param>
        /// <param name="queue">container to test fro containment of array values</param>
        static void TestContainment(int[] list, Queue<int> queue)
        {
            foreach (int num in list)
            {
                Assert.IsTrue(queue.Contains(num), "value {0} was not found within the queue", num);
            }
        }
        #endregion
    }
}