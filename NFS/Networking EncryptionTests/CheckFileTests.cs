using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class CheckFileTests
    {
        // Test constants
        const string CHECK_FILE_CAT = "Check File Tests";
        const string FILE_COMPARE = "File Compare Tests";
        const string EXCEPTION_CAT = "Exception";
        const string PROVIDER_TYPE = "Microsoft.VisualStudio.TestTools.DataSource.XML";
        const string FILE = "|DataDirectory|\\checkFileData.xml";
        const string SAME_EXT = "sameExtention";
        const string DIF_EXT = "difExtention";
        const string FILE_TO_COMPARE_ONE = "fileOne";
        const string COMPARE_FILE = "compareToFile";
        const string RESOUCE_NODE = "Resource";
        const string RESOURCE_FILE = "file";

        #region Test Intializers
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        #endregion
        [TestMethod()]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, SAME_EXT, DataAccessMethod.Sequential)]
        public void checkExtentionSameExtention()
        {
            Assert.IsTrue(CheckFile.checkExtention(TestContext.DataRow[FILE_TO_COMPARE_ONE].ToString(),
                TestContext.DataRow[COMPARE_FILE].ToString()));
        }
        [TestMethod()]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, DIF_EXT, DataAccessMethod.Sequential)]
        public void checkExtentionDifExtention()
        {
            Assert.IsFalse(CheckFile.checkExtention(TestContext.DataRow[FILE_TO_COMPARE_ONE].ToString(),
                TestContext.DataRow[COMPARE_FILE].ToString()));
        }
        [TestMethod()]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, RESOUCE_NODE, DataAccessMethod.Sequential)]
        public void checkExtentionNoExtention()
        {
            Assert.IsFalse(CheckFile.checkExtention(TestContext.DataRow[RESOURCE_FILE].ToString(),
                "this is a test"));
        }
        [TestMethod()]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, RESOUCE_NODE, DataAccessMethod.Sequential)]
        public void getExtentionHasExtentionTest()
        {
            Assert.AreEqual(TestContext.DataRow["type"].ToString(),
                CheckFile.getExtention(TestContext.DataRow[RESOURCE_FILE].ToString()));
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_CAT)]
        [TestCategory(CHECK_FILE_CAT)]
        [ExpectedException(typeof(FormatException))]
        public void getExtentionNoExtentionTest()
        {
            CheckFile.getExtention("this is a test");
        }
        [TestMethod()]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, RESOUCE_NODE, DataAccessMethod.Sequential)]
        public void checkHasExtentionHasExtention()
        {
            Assert.IsTrue(CheckFile.checkHasExtention(TestContext.DataRow[RESOURCE_FILE].ToString()));
        }
        [TestMethod()]
        [TestCategory(CHECK_FILE_CAT)]
        public void checkHasExtentionNoExtentionTest()
        {
            Assert.IsFalse(CheckFile.checkHasExtention("this is a test"));
        }
        [TestMethod()]
        [TestCategory(RESOUCE_NODE + " Tests")]
        [DataSource(PROVIDER_TYPE, FILE, RESOUCE_NODE, DataAccessMethod.Sequential)]
        public void testResourceLocations()
        {
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow[RESOURCE_FILE].ToString())));
            Assert.AreNotEqual("", TestContext.DataRow["type"].ToString());
        }
        [TestMethod()]
        [TestCategory(RESOUCE_NODE + " Tests")]
        [DataSource(PROVIDER_TYPE, FILE, SAME_EXT, DataAccessMethod.Sequential)]
        public void testResourceSameExtResource()
        {
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow[FILE_TO_COMPARE_ONE].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow[COMPARE_FILE].ToString())));
        }
        [TestMethod()]
        [TestCategory(RESOUCE_NODE + " Tests")]
        [DataSource(PROVIDER_TYPE, FILE, DIF_EXT, DataAccessMethod.Sequential)]
        public void testResourceParseOneFile()
        {
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow[FILE_TO_COMPARE_ONE].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow[COMPARE_FILE].ToString())));
        }
        [TestMethod()]
        [TestCategory(CHECK_FILE_CAT)]
        public void GetPathTestPass()
        {
            string path = "..\\..\\EncryptionTestFiles\\EncryptedPngOne.png";
            string retPath = CheckFile.GetPath(path);
            Assert.AreEqual(Directory.GetParent(path).FullName + path, CheckFile.GetPath(path));
            Assert.IsTrue(File.Exists(retPath), " file does not exsist");
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_CAT)]
        [TestCategory(CHECK_FILE_CAT)]
        [ExpectedException(typeof(FormatException))]
        public void GetPathExceptionThrown()
        {
            CheckFile.GetPath("This is a test");
        }
        #region FileCompare Tests
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, SAME_EXT, DataAccessMethod.Sequential)]
        public void FileCompareSameExtAreEqual()
        {
            string fileOne = CheckFile.GetPath(TestContext.DataRow[FILE_TO_COMPARE_ONE].ToString());
            string fileTwo = CheckFile.GetPath(TestContext.DataRow[FILE_TO_COMPARE_ONE].ToString());
            Assert.IsTrue(CheckFile.CompareFile(@fileOne, @fileTwo), "File Compare Funct");
        }
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, SAME_EXT, DataAccessMethod.Sequential)]
        public void FileCompareSameExtNotEqual()
        {
            string fileOne = CheckFile.GetPath(TestContext.DataRow[FILE_TO_COMPARE_ONE].ToString());
            string fileTwo = CheckFile.GetPath(TestContext.DataRow[COMPARE_FILE].ToString());
            Assert.IsFalse(CheckFile.CompareFile(fileOne, fileTwo), "File Compare Funct");
        }
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE, FILE, RESOUCE_NODE, DataAccessMethod.Sequential)]
        public void FileCompareSameFile()
        {
            string fileOne = CheckFile.GetPath(TestContext.DataRow[RESOURCE_FILE].ToString());
            Assert.IsTrue(CheckFile.CompareFile(fileOne, fileOne), "File Compare Funct");
        }
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        [TestCategory(CHECK_FILE_CAT)]
        [DataSource(PROVIDER_TYPE,FILE, DIF_EXT, DataAccessMethod.Sequential)]
        public void FileCompareDifExt()
        {
            string fileOne = CheckFile.GetPath(TestContext.DataRow[FILE_TO_COMPARE_ONE].ToString());
            string fileTwo = CheckFile.GetPath(TestContext.DataRow[COMPARE_FILE].ToString());
            Assert.IsFalse(CheckFile.CompareFile(fileOne, fileTwo), "File Compare Funct");
        }
        #endregion
    }
}