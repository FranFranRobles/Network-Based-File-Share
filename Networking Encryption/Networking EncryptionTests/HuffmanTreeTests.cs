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
    public class HuffmanTreeTests
    {
        #region Huffman xml node constants
        //huffman.xml node constants
        const string ONE_FILE = "OneFile";
        const string TWO_FILE = "TwoFile";
        const string RESOURCE = "Resource";
        const string DIF_FILE = "difFile";
        const string FILE_TO_COMPRESS = "fileToCompress";
        const string COMPRESSED_FILE = "compressedFile";
        const string DECOMPRESSED_FILE = "decompressedFile";
        const string PROVIDER_TYPE = "Microsoft.VisualStudio.TestTools.DataSource.XML";
        const string FILE_LOCATION = "|DataDirectory|\\Tests.xml";
        #endregion
        // test constants
        const string H_MAN_CAT = "Huffman Tree Tests";
        const string ENCODE_CONST = "!!!!!4444 AAAbbb xxz";
        static readonly string[] ENCODE_LIST = {"!","4", " ","A","b","x","z"};
        static readonly int[] ENCODE_FREQ = { 5, 4, 2, 3, 3, 2, 1 };

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        HuffmanTree tree = null;
        [TestInitialize]
        public void TestIntializer()
        {
           tree = new HuffmanTree();
        }
        #region CTOR Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void TestDefaultCTOR()
        {
            Assert.IsTrue(tree.IsNull, "Tree should be Null");
        }
        // need to figure out how to delimit different letter from different encodeds
        #endregion

        #region Encode Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void EncodeStrTest()
        {
            string encodedData = tree.Encode(ENCODE_CONST);
            TestStrEncode(encodedData);
        }

        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        [DataSource(PROVIDER_TYPE, FILE_LOCATION, ONE_FILE, DataAccessMethod.Sequential)]
        public void EncodeFileTest()
        {
            string fileToCompress = TestContext.DataRow[FILE_TO_COMPRESS].ToString();
            string compressedFile = TestContext.DataRow[COMPRESSED_FILE].ToString();
            tree.Encode(fileToCompress,COMPRESSED_FILE);
            Assert.Fail();
        }
        #endregion

        #region Decode Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void DecodeStrTest()
        {
            string encodedData = tree.Encode(ENCODE_CONST);
            TestStrEncode(encodedData);
            Assert.Fail();
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void DecodeFileTest()
        {
            Assert.Fail();
        }
        #endregion

        #region Find Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FindCharTest()
        {
            string encodedData = tree.Encode(ENCODE_CONST);
            TestStrEncode(encodedData);
            Assert.Fail();
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FindValueTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FindCharNotFoundTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FindValueNotFoundTest()
        {
            Assert.Fail();
        }
        #endregion

        #region Contains Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void ContainsCharPassTest()
        {
            string encodedData = tree.Encode(ENCODE_CONST);
            TestStrEncode(encodedData);
            Assert.Fail();
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void ContainsValPassTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void ContainsCharNotFoundTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void ContainsValNotFoundTest()
        {
            Assert.Fail();
        }
        #endregion

        #region Flush Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FlushIsNullTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FlushNotNullNullTest()
        {
            Assert.Fail();
        }
        #endregion

        #region Huffman Test Helpers
        /// <summary>
        /// Asserts that the tree is not null that the test string is found with the correct frequencies
        /// </summary>
        /// <param name="encodedData"></param>
        private void TestStrEncode(string encodedData)
        {
            Assert.IsFalse(tree.IsNull, "should not be null");
            Assert.AreNotEqual(ENCODE_CONST, encodedData, "string did not encode");
            for (int index = 0; index < ENCODE_LIST.Length; index++)
            {
                Assert.IsTrue(tree.Contains(ENCODE_LIST[index]), "should be found in the tree");
                Assert.AreEqual(ENCODE_FREQ[index], tree.Find(ENCODE_LIST[index]), "Incorrect freq Returned");
            }
        }
        #endregion
    }
}