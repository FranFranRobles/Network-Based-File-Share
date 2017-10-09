using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
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
        #region Test Constants
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
        const string ONE_KEYWORD = "One";
        const string TWO_KEYWORD = "Two";
        // test constants
        const string H_MAN_CAT = "Huffman Tree Tests";
        const string COMPRESSION_STR = "!!!!!4444 AAAbbb xxz";
        static readonly string[] ENCODE_LIST = { "!", "4", " ", "A", "b", "x", "z" };
        static readonly int[] ENCODE_FREQ = { 5, 4, 2, 3, 3, 2, 1 };
        #endregion

        #region Test intializer
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
        #endregion

        #region CTOR Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void TestDefaultCTOR()
        {
            Assert.IsTrue(tree.IsNull, "Tree should be Null");
        }
        #endregion

        #region Compression Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void CompressStrTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            TestStrEncode(encodedData);
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        [DataSource(PROVIDER_TYPE, FILE_LOCATION, ONE_FILE, DataAccessMethod.Sequential)]
        public void CompressFileTest()
        {
            string fileToCompress = TestContext.DataRow[FILE_TO_COMPRESS].ToString();
            string compressedFile = TestContext.DataRow[COMPRESSED_FILE].ToString();
            tree.Compress(fileToCompress, compressedFile);
            TestFileCompression(fileToCompress, compressedFile);
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        [DataSource(PROVIDER_TYPE, FILE_LOCATION, TWO_FILE, DataAccessMethod.Sequential)]
        public void CompressFileSameFileTest()
        {
            string fileToCompressOne = TestContext.DataRow[FILE_TO_COMPRESS + ONE_KEYWORD].ToString();
            string fileToCompressTwo = TestContext.DataRow[FILE_TO_COMPRESS + TWO_KEYWORD].ToString();
            string compressedFileOne = TestContext.DataRow[COMPRESSED_FILE + ONE_KEYWORD].ToString();
            string compressedFileTwo = TestContext.DataRow[COMPRESSED_FILE + TWO_KEYWORD].ToString();
            tree.Compress(fileToCompressOne, compressedFileOne);
            tree.Compress(fileToCompressTwo, compressedFileTwo);
            TestSameFileCompression(fileToCompressOne, fileToCompressTwo, compressedFileOne, compressedFileTwo);
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        [DataSource(PROVIDER_TYPE, FILE_LOCATION, DIF_FILE, DataAccessMethod.Sequential)]
        public void CompressFileDifFileTest()
        {
            string fileToCompressOne = TestContext.DataRow[FILE_TO_COMPRESS + ONE_KEYWORD].ToString();
            string fileToCompressTwo = TestContext.DataRow[FILE_TO_COMPRESS + TWO_KEYWORD].ToString();
            string compressedFileOne = TestContext.DataRow[COMPRESSED_FILE + ONE_KEYWORD].ToString();
            string compressedFileTwo = TestContext.DataRow[COMPRESSED_FILE + TWO_KEYWORD].ToString();
            tree.Compress(fileToCompressOne, compressedFileOne);
            tree.Compress(fileToCompressTwo, compressedFileTwo);
            TestDifFileCompression(fileToCompressOne, fileToCompressTwo, compressedFileOne, compressedFileTwo);
        }

        #endregion

        #region Decompression Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void DecompressStrTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            string decodedStr = tree.Decompress(encodedData);
            TestStrEncode(encodedData);
            Assert.AreEqual(true, encodedData.Length < decodedStr.Length, "decode len should be longer");
            Assert.AreEqual(COMPRESSION_STR.Length, decodedStr.Length, "Length not the same as the orginal");
            Assert.AreEqual(COMPRESSION_STR, decodedStr, "strings are different from each other");
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        [DataSource(PROVIDER_TYPE, FILE_LOCATION, ONE_FILE, DataAccessMethod.Sequential)]
        public void DecompressFileTest()
        {
            string fileToCompress = TestContext.DataRow[FILE_TO_COMPRESS].ToString();
            string compressedFile = TestContext.DataRow[COMPRESSED_FILE].ToString();
            string decompressed = TestContext.DataRow[DECOMPRESSED_FILE].ToString();
            tree.Compress(fileToCompress, compressedFile);
            TestFileCompression(fileToCompress, compressedFile);
            tree.Decompress(compressedFile, decompressed);
            TestFileCompression(decompressed, compressedFile);
            Assert.IsTrue(CheckFile.CompareFile(fileToCompress, decompressed), "Files are not the same");
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        [DataSource(PROVIDER_TYPE, FILE_LOCATION, TWO_FILE, DataAccessMethod.Sequential)]
        public void DecompressFileSameFileTest()
        {
            string fileToCompressOne = TestContext.DataRow[FILE_TO_COMPRESS + ONE_KEYWORD].ToString();
            string fileToCompressTwo = TestContext.DataRow[FILE_TO_COMPRESS + TWO_KEYWORD].ToString();
            string compressedFileOne = TestContext.DataRow[COMPRESSED_FILE + ONE_KEYWORD].ToString();
            string compressedFileTwo = TestContext.DataRow[COMPRESSED_FILE + TWO_KEYWORD].ToString();
            string decompressedOne = TestContext.DataRow[DECOMPRESSED_FILE + ONE_KEYWORD].ToString();
            string decompressedTwo = TestContext.DataRow[DECOMPRESSED_FILE + TWO_KEYWORD].ToString();
            tree.Compress(fileToCompressOne, compressedFileOne);
            tree.Compress(fileToCompressTwo, compressedFileTwo);
            TestSameFileCompression(fileToCompressOne, fileToCompressTwo, compressedFileOne, compressedFileTwo);
            tree.Decompress(compressedFileOne, decompressedOne);
            tree.Decompress(compressedFileTwo, decompressedTwo);
            TestSameFileCompression(decompressedOne, decompressedTwo, compressedFileOne, compressedFileTwo);
            Assert.IsTrue(CheckFile.CompareFile(fileToCompressOne, decompressedOne), "Files are not the same");
            Assert.IsTrue(CheckFile.CompareFile(fileToCompressTwo, decompressedTwo), "Files are not the same");
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        [DataSource(PROVIDER_TYPE, FILE_LOCATION, DIF_FILE, DataAccessMethod.Sequential)]
        public void DecompressFileDifFileTest()
        {
            string fileToCompressOne = TestContext.DataRow[FILE_TO_COMPRESS + ONE_KEYWORD].ToString();
            string fileToCompressTwo = TestContext.DataRow[FILE_TO_COMPRESS + TWO_KEYWORD].ToString();
            string compressedFileOne = TestContext.DataRow[COMPRESSED_FILE + ONE_KEYWORD].ToString();
            string compressedFileTwo = TestContext.DataRow[COMPRESSED_FILE + TWO_KEYWORD].ToString();
            string decompressedOne = TestContext.DataRow[DECOMPRESSED_FILE + ONE_KEYWORD].ToString();
            string decompressedTwo = TestContext.DataRow[DECOMPRESSED_FILE + TWO_KEYWORD].ToString();
            tree.Compress(fileToCompressOne, compressedFileOne);
            tree.Compress(fileToCompressTwo, compressedFileTwo);
            TestDifFileCompression(fileToCompressOne, fileToCompressTwo, compressedFileOne, compressedFileTwo);
            tree.Decompress(compressedFileOne, decompressedOne);
            tree.Decompress(compressedFileTwo, decompressedTwo);
            TestDifFileCompression(decompressedOne, decompressedTwo, compressedFileOne, compressedFileTwo);
            Assert.IsTrue(CheckFile.CompareFile(fileToCompressOne, decompressedOne), "Files are not the same");
            Assert.IsTrue(CheckFile.CompareFile(fileToCompressTwo, decompressedTwo), "Files are not the same");
        }
        #endregion

        #region Find Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FindCharTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            TestStrEncode(encodedData);
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FindValueTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            Assert.IsFalse(tree.IsNull, "should not be null");
            Assert.AreNotEqual(COMPRESSION_STR, encodedData, "string did not encode");
            for (int index = 0; index < ENCODE_LIST.Length; index++)
            {
                Assert.IsTrue(tree.Contains(ENCODE_LIST[index]), "should be found in the tree");
                Assert.AreEqual(ENCODE_FREQ[index], tree.Find(Convert.ToByte(ENCODE_LIST[index])),
                    "Incorrect freq Returned");
            }
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FindCharNotFoundTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            Assert.IsFalse(tree.IsNull, "should not be null");
            Assert.AreNotEqual(COMPRESSION_STR, encodedData, "string did not encode");
            for (int index = 0; index < 5; index++)
            {
                char curr = Convert.ToChar('k' + index);
                Assert.IsFalse(tree.Contains(curr.ToString()), "should not be found in the tree");
                Assert.AreEqual(0, tree.Find(curr.ToString()), "Incorrect freq Returned");
            }
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FindValueNotFoundTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            Assert.IsFalse(tree.IsNull, "should not be null");
            Assert.AreNotEqual(COMPRESSION_STR, encodedData, "string did not encode");
            byte valTofind = 107;
            for (int index = 0; index < 5; index++)
            { 
                Assert.IsFalse(tree.Contains(valTofind), "should not be found in the tree");
                Assert.AreEqual(0, tree.Find(valTofind), "Incorrect freq Returned");
                valTofind++;
            }
        }
        #endregion

        #region Contains Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void ContainsCharPassTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            TestStrEncode(encodedData);
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void ContainsValPassTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            Assert.IsFalse(tree.IsNull, "should not be null");
            Assert.AreNotEqual(COMPRESSION_STR, encodedData, "string did not encode");
            for (int index = 0; index < ENCODE_LIST.Length; index++)
            {
                Assert.IsTrue(tree.Contains(ENCODE_LIST[index]), "should be found in the tree");
                Assert.AreEqual(ENCODE_FREQ[index], tree.Find(Convert.ToByte(ENCODE_LIST[index])),
                    "Incorrect freq Returned");
            }
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void ContainsCharNotFoundTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            Assert.IsFalse(tree.IsNull, "should not be null");
            Assert.AreNotEqual(COMPRESSION_STR, encodedData, "string did not encode");
            for (int index = 0; index < 5; index++)
            {
                char curr = Convert.ToChar('k' + index);
                Assert.IsFalse(tree.Contains(curr.ToString()), "should not be found in the tree");
                Assert.AreEqual(0, tree.Find(curr.ToString()), "Incorrect freq Returned");
            }
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void ContainsValNotFoundTest()
        {
            string encodedData = tree.Compress(COMPRESSION_STR);
            Assert.IsFalse(tree.IsNull, "should not be null");
            Assert.AreNotEqual(COMPRESSION_STR, encodedData, "string did not encode");
            byte valTofind = 107;
            for (int index = 0; index < 5; index++)
            {
                Assert.IsFalse(tree.Contains(valTofind), "should not be found in the tree");
                Assert.AreEqual(0, tree.Find(valTofind), "Incorrect freq Returned");
                valTofind++;
            }
        }
        #endregion

        #region Flush Tests
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FlushIsNullTest()
        {
            Assert.IsTrue(tree.IsNull, "should be null");
            tree.Flush();
            Assert.IsTrue(tree.IsNull, "should be null");
        }
        [TestMethod()]
        [TestCategory(H_MAN_CAT)]
        public void FlushNotNullNullTest()
        {
            tree.Compress(COMPRESSION_STR);
            Assert.IsFalse(tree.IsNull, "should be null");
            tree.Flush();
            Assert.IsTrue(tree.IsNull, "should be null");
        }
        #endregion

        #region Huffman Test Helpers
        /// <summary>
        /// Asserts that the tree is not null that the test string is found with the correct frequencies
        /// </summary>
        /// <param name="encodedData"></param>
        public void TestStrEncode(string encodedData)
        {
            Assert.IsFalse(tree.IsNull, "should not be null");
            Assert.AreNotEqual(COMPRESSION_STR, encodedData, "string did not encode");
            for (int index = 0; index < ENCODE_LIST.Length; index++)
            {
                Assert.IsTrue(tree.Contains(ENCODE_LIST[index]), "should be found in the tree");
                Assert.AreEqual(ENCODE_FREQ[index], tree.Find(ENCODE_LIST[index]), "Incorrect freq Returned");
            }
        }
        /// <summary>
        /// Tests to see Compression occured
        /// </summary>
        /// <param name="fileToCompress">intial state of the file</param>
        /// <param name="compressedFile">compressed file</param>
        public void TestFileCompression(string fileToCompress, string compressedFile)
        {
            Assert.IsFalse(CheckFile.CompareFile(fileToCompress, compressedFile),
                "Did not compress Correctly");
            Assert.AreEqual(true, new FileInfo(fileToCompress).Length > new FileInfo(compressedFile).Length,
                "file did not decrease in size");
        }
        /// <summary>
        /// test whether two equal files compressed the same
        /// </summary>
        /// <param name="uncompressedOne">first starting path</param>
        /// <param name="uncompressedTwo">second starting path</param>
        /// <param name="compressedOne">first compressed file</param>
        /// <param name="compressedTwo">second compressed file</param>
        public void TestSameFileCompression(string uncompressedOne, string uncompressedTwo,
            string compressedOne, string compressedTwo)
        {
            TestFileCompression(uncompressedOne, compressedOne);
            TestFileCompression(uncompressedTwo, compressedTwo);
            Assert.IsTrue(CheckFile.CompareFile(uncompressedOne, uncompressedTwo),
                " They are not the same files");
            Assert.IsTrue(CheckFile.CompareFile(compressedOne, compressedTwo),
                " Files did not compress in the same format");
        }
        /// <summary>
        /// test to make sure that two different files compress differently
        /// </summary>
        /// <param name="uncompressedOne">first starting path</param>
        /// <param name="uncompressedTwo">second starting path</param>
        /// <param name="compressedOne">first compressed file</param>
        /// <param name="compressedTwo">second compressed file</param>
        public void TestDifFileCompression(string uncompressedOne, string uncompressedTwo,
            string compressedOne, string compressedTwo)
        {
            TestFileCompression(uncompressedOne, compressedOne);
            TestFileCompression(uncompressedTwo, compressedTwo);
            Assert.IsFalse(CheckFile.CompareFile(uncompressedOne, uncompressedTwo),
                " Files are not different");
            Assert.IsFalse(CheckFile.CompareFile(compressedOne, compressedTwo),
                " Files did not compress differently");
        }
        #endregion
    }
}