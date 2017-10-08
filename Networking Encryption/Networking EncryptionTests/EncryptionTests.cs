using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Resources;
/*
 * CodeMetrics: 64  83  1   15  546
 */
namespace Networking_Encryption.Tests
{

    [TestClass()]
    public class EncryptionTests
    {
        #region Test Constants
        // Test Constants
        const string ENCRYPT_TESTS = "Encryption Tests";
        const string DECRYPT_TESTS = "Decryption Tests";
        const string RESOURCE_TESTS = "Resource Tests";
        //class constants
        const string TEST_MSG = "this is a test";
        const string KEY_ONE = "3";
        const string KEY_TWO = "4";
        const string SEED_ONE = "1";
        const string SEED_TWO = "2";
        #endregion

        #region Test Intalization
        private Encryption encryptor = null;
        private KeyHolder keyOne = null;
        private KeyHolder keyTwo = null;

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        [TestInitialize]
        public void TestIntialize()
        {
            encryptor = new Encryption();
            keyOne = new KeyHolder();
            keyTwo = new KeyHolder();
        }
        #endregion

        #region Encryption Tests

        #region Encryption Random Encrpt Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStrTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(cipherText);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStr()
        {
            string EncryptedText = encryptor.CompressEncrypt(TEST_MSG, ref keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(EncryptedText);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void EncryptFileTest()
        {
            string fileToEncrypt = CheckFile.GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string encryptedFile = CheckFile.GetPath(TestContext.DataRow["encryptedFile"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecrypt(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFile()
        {
            string fileToEncrypt = CheckFile.GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string saveDestination = CheckFile.GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt, saveDestination);
            TestEncryptDecrypt(fileToEncrypt, saveDestination);
            checkPairNotNull(keyOne);
        }
        #endregion

        #region Same Seeds Diff Keys Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStrSameSeedDifKeyTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE, KEY_TWO);
            TestStrEncryption(cipherText1, cipherText2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrSameSeedDifKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_ONE, KEY_TWO);
            TestStrEncryption(cipherText1, cipherText2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void EncryptFileSameSeedDifKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void EncryptDifFileSameSeedDifKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void CompressEncryptDifFileSameSeedDifKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFileSameSeedDifKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        #endregion

        #region Dif Seed Same Key Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStrDifSeedSameKeyTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_TWO, KEY_ONE);
            TestStrEncryption(cipherText1, cipherText2);
            TestDifSeedSameKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrDifSeedSameKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_TWO, KEY_ONE);
            TestStrEncryption(cipherText1, cipherText2);
            TestDifSeedSameKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void EncryptFileDifSeedSameKeyTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestFileEncryption(fileToEncrypt1,encryptedFile1,fileToEncrypt2,encryptedFile2);
            TestDifSeedSameKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void EncryptDifFileDifSeedSameKeyTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedSameKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void CompressEncryptDifFileDifSeedSameKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedSameKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFileDifSeedSameKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedSameKey(keyOne, keyTwo);
        }
        #endregion

        #region Different Seed  & Key Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStrDifSeedKeyTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo);
            TestStrEncryption(cipherText1, cipherText2);
            TestDifSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrDifSeedKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo);
            TestStrEncryption(cipherText1, cipherText2);
            TestDifSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void EncryptFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestFileEncryption(fileToEncrypt1,encryptedFile1,fileToEncrypt2,encryptedFile2);
            TestDifSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
             "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void EncryptDifFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void CompressEncryptDifFileDifSeedKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFileDifSeedKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedKey(keyOne, keyTwo);
        }
        #endregion

        #region Same Seed  & Key Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStrSameSeedKeyTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE, KEY_ONE);
            TestStrEncryptionSameSeedKey(cipherText1, cipherText2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrSameSeedKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_ONE, KEY_ONE);
            TestStrEncryptionSameSeedKey(cipherText1, cipherText2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void EncryptFileSameSeedKeyTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestFileEncryptionSameSeedKey(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void EncryptDifFileSameSeedKeyTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void CompressEncryptDifFileSameSeedKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFileSameSeedKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestFileEncryptionSameSeedKey(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        #endregion
        #endregion

        #region Decryption Tests

        #region Random Decrypt Tests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStrTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            string decipherdText = encryptor.Decrypt(cipherText, keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(cipherText);
            Assert.AreEqual(TEST_MSG, decipherdText, "Msgs should be equal");
            Assert.AreNotEqual(cipherText, decipherdText, "Failed To Decrypt Msg");
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStr()
        {
            string cipherText = encryptor.CompressEncrypt(TEST_MSG, ref keyOne);
            string decipherdText = encryptor.DecompressDecrypt(cipherText, keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(cipherText);
            Assert.AreEqual(TEST_MSG, decipherdText, "Msgs should be equal");
            Assert.AreNotEqual(cipherText, decipherdText, "Failed To Decrypt Msg");
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void DecryptFileTest()
        {
            string fileToEncrypt = CheckFile.GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string encryptedFile = CheckFile.GetPath(TestContext.DataRow["encryptedFile"].ToString());
            string decryptedFile = CheckFile.GetPath(TestContext.DataRow["decryptedFile"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecrypt(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
            encryptor.Decrypt(encryptedFile, decryptedFile, keyOne);
            TestEncryptDecrypt(decryptedFile,encryptedFile);
            Assert.IsTrue(CheckFile.CompareFile(fileToEncrypt, decryptedFile),
                "Decrypted File Not Same Len As Orginal File");
        } 
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFile()
        {
            string fileToEncrypt = CheckFile.GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string encryptedFile = CheckFile.GetPath(TestContext.DataRow["encryptedFile"].ToString());
            string decryptedFile = CheckFile.GetPath(TestContext.DataRow["decryptedFile"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecrypt(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
            encryptor.DecompressDecrypt(encryptedFile, decryptedFile, keyOne);
            TestEncryptDecrypt(decryptedFile, encryptedFile);
            Assert.IsTrue(CheckFile.CompareFile(fileToEncrypt, decryptedFile),
                "Decrypted File Not Same Len As Orginal File");
        }
        #endregion

        #region Same Seed Dif KeysTests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStrSameSeedDifKeyTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE, KEY_TWO);
            TestStrEncryption(cipherText1, cipherText2);
            checkSameSeedDifKey(keyOne, keyTwo);
            TestStrDecryption(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, keyOne),
                encryptor.Decrypt(cipherText1, keyOne));
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStrSameSeedDifKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_ONE, KEY_TWO);
            TestStrEncryption(cipherText1, cipherText2);
            checkSameSeedDifKey(keyOne, keyTwo);
            TestStrDecryption(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1, keyOne),
                encryptor.DecompressDecrypt(cipherText1, keyOne));
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptFileSameSeedDifKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, 
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
    "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void DecryptDifFileSameSeedDifKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryptionDifFiles(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void CompressDecryptDifFileSameSeedDifKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryptionDifFiles(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFileSameSeedDifKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        #endregion

        #region Dif Seed same key Tests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStrDifSeedSameKey()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_TWO, KEY_ONE);
            TestStrEncryption(cipherText1, cipherText2);
            TestDifSeedSameKey(keyOne, keyTwo);
            TestStrDecryption(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, 
                keyOne), encryptor.Decrypt(cipherText1, keyOne));
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStrDifSeedSameKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_TWO, KEY_ONE);
            TestStrEncryption(cipherText1, cipherText2);
            TestDifSeedSameKey(keyOne, keyTwo);
            TestStrDecryption(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1,
                keyOne), encryptor.DecompressDecrypt(cipherText1, keyOne));
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptFileDifSeedSameKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedSameKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
    "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void DecryptDifFileDifSeedSameKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedSameKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryptionDifFiles(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void CompressDecryptDifFileDifSeedSameKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedSameKey(keyOne, keyTwo);
            encryptor.DecompressDecrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.DecompressDecrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryptionDifFiles(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFileDifSeedSameKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedSameKey(keyOne, keyTwo);
            encryptor.DecompressDecrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.DecompressDecrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        #endregion

        #region Dif Seed & Key tests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStrDifSeedKeyTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo);
            TestStrEncryption(cipherText1, cipherText2);
            TestDifSeedKey(keyOne, keyTwo);
            TestStrDecryption(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, 
                keyOne), encryptor.Decrypt(cipherText1, keyOne));
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStrDifSeedKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo);
            TestStrEncryption(cipherText1, cipherText2);
            TestDifSeedKey(keyOne, keyTwo);
            TestStrDecryption(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1,
                keyOne), encryptor.DecompressDecrypt(cipherText1, keyOne));
            TestDifSeedSameKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
    "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void DecryptDifFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryptionDifFiles(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void CompressDecryptDifFileDifSeedKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedKey(keyOne, keyTwo);
            encryptor.DecompressDecrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.DecompressDecrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryptionDifFiles(fileToEncrypt1, fileToEncrypt2, encryptedFile1,
                encryptedFile2, decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFileDifSeedKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2);
            TestFileEncryption(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            TestDifSeedKey(keyOne, keyTwo);
            encryptor.DecompressDecrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.DecompressDecrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, 
                encryptedFile2, decryptedFile1, decryptedFile2);
        }
        #endregion

        #region Same Seed & Key tests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStrSameKeySeedTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE,  KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE, KEY_ONE);
            TestStrEncryptionSameSeedKey(cipherText1, cipherText2);
            checkSameSeedKey(keyOne, keyTwo);
            TestStrDecryption(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, 
                keyOne), encryptor.Decrypt(cipherText1, keyOne));
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStrSameSeedKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_ONE, KEY_ONE);
            TestStrEncryptionSameSeedKey(cipherText1, cipherText2);
            checkSameSeedKey(keyOne, keyTwo);
            TestStrDecryption(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1,
                keyOne), encryptor.CompressEncrypt(cipherText1, ref keyOne));
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptFileSameKeySeedTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestFileEncryptionSameSeedKey(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void DecryptDifFileSameKeySeedTest()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestFileEncryptionDifFileTypes(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryptionDifFiles(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void CompressDecryptDifFileSameSeedKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestFileEncryptionSameSeedKey(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryptionDifFiles(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFileSameSeedKey()
        {
            string fileToEncrypt1 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestFileEncryptionSameSeedKey(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
            encryptor.DecompressDecrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.DecompressDecrypt(encryptedFile2, decryptedFile2, keyTwo);
            TestFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2,
                decryptedFile1, decryptedFile2);
        }
        #endregion
        #endregion

        #region Resource Tests
        [TestMethod()]
        [TestCategory(RESOURCE_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "Resource", DataAccessMethod.Sequential)]
        public void testResourceLocations()
        {
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["file"].ToString())));
        }
        [TestMethod()]
        [TestCategory(RESOURCE_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
           "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void testResourceParseOneFile()
        {
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["fileToEncrypt"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["encryptedFile"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["decryptedFile"].ToString())));
        }
        [TestMethod()]
        [TestCategory(RESOURCE_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void testResourceParseTwoFile()
        {
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString())));
        }
        [TestMethod()]
        [TestCategory(RESOURCE_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "MixFile", DataAccessMethod.Sequential)]
        public void testResourceParseMixFile()
        {
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["fileToEncryptOne"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["encryptedFileOne"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["decryptedFileOne"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["encryptedFileTwo"].ToString())));
            Assert.IsTrue(File.Exists(CheckFile.GetPath(TestContext.DataRow["decryptedFileTwo"].ToString())));
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// asserts that the given class is not null referenced
        /// </summary>
        /// <param name="keys"></param>
        static void checkPairNotNull(KeyHolder keys)
        {
            Assert.IsNotNull(keys);
            Assert.IsNotNull(keys.Key);
            Assert.IsNotNull(keys.Seed);
        }
        /// <summary>
        /// function asserts the following: that the given pairs are not null, encryption mode and seeds are equal and that the keys are diferent
        /// </summary>
        /// <param name="keyOne">first pair to check</param>
        /// <param name="keyTwo">second pair to check</param>
        static void checkSameSeedDifKey(KeyHolder keyOne, KeyHolder keyTwo)
        {
            checkPairNotNull(keyOne);
            checkPairNotNull(keyTwo);
            Assert.AreNotEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key), "Keys are not different");
            Assert.AreEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed), " Seeds Should be equal");
        }
        /// <summary>
        /// function asserts the following: given pairs not null,encryption modes are all equal and that seed and key are different.
        /// </summary>
        /// <param name="keyOne">first pair to check</param>
        /// <param name="keyTwo">second pair to check</param>
        static void TestDifSeedSameKey(KeyHolder keyOne, KeyHolder keyTwo)
        {
            checkPairNotNull(keyOne);
            checkPairNotNull(keyTwo);
            Assert.AreEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key)," keys are different from eachther");
            Assert.AreNotEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed),"seeds should be different from eachother");
        }
        /// <summary>
        /// function tests whether the given two keys and seeds are equal to each other
        /// </summary>
        /// <param name="keyOne">key one to check</param>
        /// <param name="keyTwo">key two to check</param>
        static void TestDifSeedKey(KeyHolder keyOne, KeyHolder keyTwo)
        {
            checkPairNotNull(keyOne);
            checkPairNotNull(keyTwo);
            Assert.AreNotEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key), " keys are different from eachther");
            Assert.AreNotEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed), "seeds should be different from eachother");
        }
        /// <summary>
        /// checks if the given two keys are eqaul to eachother
        /// </summary>
        /// <param name="keyOne"></param>
        /// <param name="keyTwo"></param>
        static void checkSameSeedKey(KeyHolder keyOne, KeyHolder keyTwo)
        {
            checkPairNotNull(keyOne);
            checkPairNotNull(keyTwo);
            Assert.AreEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key), " keys not equal to each other");
            Assert.AreEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed), "seeds not equal to each other");
        }
        /// <summary>
        /// function tests that the given two files are not the same and have different lengths from each other
        /// </summary>
        /// <param name="decryptedFile"> unencrypted file</param>
        /// <param name="encryptedFile">encryptedFile</param>
        static void TestEncryptDecrypt(string decryptedFile, string encryptedFile)
        {
            Assert.IsFalse(CheckFile.CompareFile(decryptedFile, encryptedFile), "Files did not convert");
            Assert.IsTrue(new FileInfo(encryptedFile).Length > new FileInfo(decryptedFile).Length,"File was not Salted");
        }
        /// <summary>
        ///         /// function tests that the given string encrypted
        /// </summary>
        /// <param name="cipherText">Encrypted Text</param>
        private static void TestEncryption(string cipherText)
        {
            Assert.AreNotEqual(TEST_MSG, cipherText, "Text Did Not Encrypt");
            Assert.IsTrue(cipherText.Count() > TEST_MSG.Count(), "Data was not Salted");
        }
        /// <summary>
        /// Function test to make sure that the files to encrypt are the same and that the encrypted files are not the same
        /// </summary>
        /// <param name="fileToEncrypt1">first file to encrypt</param>
        /// <param name="encryptedFile1">fist encrypted file</param>
        /// <param name="fileToEncrypt2">second file to encrypt</param>
        /// <param name="encryptedFile2">second encrypted file</param>
        private static void TestFileEncryption(string fileToEncrypt1, string encryptedFile1, string fileToEncrypt2, string encryptedFile2)
        {
            TestEncryptDecrypt(fileToEncrypt1, encryptedFile1);
            TestEncryptDecrypt(fileToEncrypt2, encryptedFile2);
            Assert.IsTrue(CheckFile.CompareFile(fileToEncrypt1, fileToEncrypt2), "Files are not the same");
            Assert.IsFalse(CheckFile.CompareFile(encryptedFile1, encryptedFile2), "Encrypted Files are the same");
        }
        /// <summary>
        ///  Tests whether the same two files where encrypted the same
        /// </summary>
        /// <param name="fileToEncrypt1">file to be encrypted one</param>
        /// <param name="encryptedFile1">first encrypted file</param>
        /// <param name="fileToEncrypt2">file to be encrypted two</param>
        /// <param name="encryptedFile2">second encrypted file</param>
        private static void TestFileEncryptionSameSeedKey(string fileToEncrypt1, string encryptedFile1, string fileToEncrypt2, string encryptedFile2)
        {
            TestEncryptDecrypt(fileToEncrypt1, encryptedFile1);
            TestEncryptDecrypt(fileToEncrypt2, encryptedFile2);
            Assert.IsTrue(CheckFile.CompareFile(fileToEncrypt1, fileToEncrypt2), "Files are not the same");
            Assert.IsTrue(CheckFile.CompareFile(encryptedFile1, encryptedFile2), "Encrypted Files are not the same");
        }
        /// <summary>
        /// Tests the encryption of two files of different types
        /// </summary>
        /// <param name="fileToEncrypt1">file one to check</param>
        /// <param name="encryptedFile1">encrypted file one to check</param>
        /// <param name="fileToEncrypt2">file two to check</param>
        /// <param name="encryptedFile2">encrypted file two to check</param>
        private static void TestFileEncryptionDifFileTypes(string fileToEncrypt1, string encryptedFile1, string fileToEncrypt2, string encryptedFile2)
        {
            TestEncryptDecrypt(fileToEncrypt1, encryptedFile1);
            TestEncryptDecrypt(fileToEncrypt2, encryptedFile2);
            Assert.IsFalse(CheckFile.CompareFile(fileToEncrypt1, fileToEncrypt2), "Files are the same");
            Assert.IsFalse(CheckFile.CompareFile(encryptedFile1, encryptedFile2), "Encrypted Files are the same");
        }
        /// <summary>
        /// function asserts that decrypted texts are the same and that they are different from their encrypted versions
        /// </summary>
        /// <param name="encryptedText1">encrypted string one</param>
        /// <param name="encryptedText2">encrypted string two</param>
        /// <param name="decryptdTxt1">decrytped string one</param>
        /// <param name="decryptdTxt2">decrypted string two</param>
        static void TestStrDecryption(string encryptedText1, string encryptedText2, string decryptdTxt1, string decryptdTxt2)
        {
            Assert.AreEqual(TEST_MSG, decryptdTxt1, "Text Not The Same");
            Assert.AreEqual(decryptdTxt1, decryptdTxt2, "Text Not The Same");
            Assert.AreNotEqual(encryptedText1, decryptdTxt1, "Text Are The Same");
            Assert.AreNotEqual(encryptedText2, decryptdTxt1, "Text Are The Same");
        }
        /// <summary>
        /// function asserts that the encrypted files are not the same and that the decrypted files are all the same
        /// </summary>
        /// <param name="fileToEncrypt1">file to encrypt one</param>
        /// <param name="fileToEncrypt2">file to encrypt two</param>
        /// <param name="encryptedFile1">encrypted file one</param>
        /// <param name="encryptedFile2">encrypted file two</param>
        /// <param name="decryptedFile1">decrypted file one</param>
        /// <param name="decryptedFile2">decrypted file two</param>
        private static void TestFileDecryption(string fileToEncrypt1, string fileToEncrypt2, string encryptedFile1, string encryptedFile2, string decryptedFile1, string decryptedFile2)
        {
            Assert.IsFalse(CheckFile.CompareFile(encryptedFile1, decryptedFile1), "Files are the same");
            Assert.IsFalse(CheckFile.CompareFile(encryptedFile2, decryptedFile2), "Files are the same");
            Assert.IsTrue(CheckFile.CompareFile(fileToEncrypt1, decryptedFile1), "Files are not the same");
            Assert.IsTrue(CheckFile.CompareFile(fileToEncrypt2, decryptedFile2), "Files are not the same");
            Assert.IsTrue(CheckFile.CompareFile(decryptedFile1, decryptedFile2), "Files are not the same");
        }
        /// <summary>
        /// Tests whether two files with different extensions encrypt and decrypt properly
        /// </summary>
        /// <param name="fileToEncrypt1">file to encrypt one</param>
        /// <param name="fileToEncrypt2">file to encrypt two</param>
        /// <param name="encryptedFile1">encrypted file one</param>
        /// <param name="encryptedFile2">encrypted file two</param>
        /// <param name="decryptedFile1">decrypted file one</param>
        /// <param name="decryptedFile2">decrypted file two</param>
        private static void TestFileDecryptionDifFiles(string fileToEncrypt1, string fileToEncrypt2, string encryptedFile1, string encryptedFile2, string decryptedFile1, string decryptedFile2)
        {
            Assert.IsFalse(CheckFile.CompareFile(encryptedFile1, decryptedFile1), "Files are the same");
            Assert.IsFalse(CheckFile.CompareFile(encryptedFile2, decryptedFile2), "Files are the same");
            Assert.IsTrue(CheckFile.CompareFile(fileToEncrypt1, decryptedFile1), "Files are not the same");
            Assert.IsTrue(CheckFile.CompareFile(fileToEncrypt2, decryptedFile2), "Files are not the same");
            Assert.IsFalse(CheckFile.CompareFile(decryptedFile1, decryptedFile2), "Files are the same");
        }
        /// <summary>
        /// function asserts given strings encrypted and that they did not
        /// encrypt to the same thing
        /// </summary>
        /// <param name="cipherText1">encrypted Msg One</param>
        /// <param name="cipherText2">encrypted Msg Two</param>
        private static void TestStrEncryption(string cipherText1, string cipherText2)
        {
            TestEncryption(cipherText1);
            TestEncryption(cipherText2);
            Assert.AreNotEqual(cipherText1, cipherText2, "Msgs should be different");
        }
        /// <summary>
        /// tests whether the two strings where encrypted the sameway
        /// </summary>
        /// <param name="cipherText1"></param>
        /// <param name="cipherText2"></param>
        private static void TestStrEncryptionSameSeedKey(string cipherText1, string cipherText2)
        {
            TestEncryption(cipherText1);
            TestEncryption(cipherText2);
            Assert.AreEqual(cipherText1, cipherText2, "Msgs should be different");
        }
        #endregion
    }
}