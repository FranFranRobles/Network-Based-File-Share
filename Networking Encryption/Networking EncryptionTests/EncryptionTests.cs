using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Resources;
using Networking_EncryptionTests.Properties;
/*
 * CodeMetrics: 61  93  1   12  850
 */
namespace Networking_Encryption.Tests
{

    [TestClass()]
    public class EncryptionTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext{
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        // Test Constants
        const string ENCRYPT_TESTS = "Encryption Tests";
        const string FILE_COMPARE = "File Compare Tests";
        const string DECRYPT_TESTS = "Decryption Tests";
        const string RESOURCE_TESTS = "Resource Tests";
        //class constants
        const string TEST_MSG = "this is a test";
        const string KEY_ONE = "A";
        const string KEY_TWO = "b";
        const string SEED_ONE = "1";
        const string SEED_TWO = "2";

        #region Test Intalization
        private Encryption encryptor = null;
        private KeyHolder keyOne = null;
        private KeyHolder keyTwo = null;

        [TestInitialize]
        public void TestIntialize()
        {
            encryptor = new Encryption();
            keyOne = new KeyHolder();
            keyTwo = new KeyHolder();
        }
        #endregion

        #region FileCompare Tests
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void FileCompareAreEqual()
        {
            string fileOne = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string fileTwo = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            Assert.IsTrue(Encryption.FileCompare(@fileOne, @fileTwo), "File Compare Funct");
        }
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void FileCompareSameFile()
        {
            string fileOne = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            Assert.IsTrue(Encryption.FileCompare(fileOne, fileOne), "File Compare Funct");
        }
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void FileCompareNotEqual()
        {
            string fileOne = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string fileTwo = GetPath(TestContext.DataRow["decryptedFile"].ToString());
            Assert.IsFalse(Encryption.FileCompare(fileOne, fileTwo), "File Compare Funct");
        }
        #endregion

        #region Encryption Tests

        #region Encryption Random Encrpt Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStringTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(cipherText);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void EncryptTxtFileTest()
        {
            string fileToEncrypt = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string encryptedFile = GetPath(TestContext.DataRow["encryptedFile"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStr()
        {
            keyOne = null;
            string EncryptedText = encryptor.CompressEncrypt(TEST_MSG, ref keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(EncryptedText);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "OneFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFile()
        {
            string fileToEncrypt = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string saveDestination = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keyOne);
        }
        #endregion

        #region Same Seeds Diff Keys Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStringSameSeedTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo, 
                SEED_ONE, KEY_TWO), "Texts should be different");
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void EncryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrSameSeed()
        {
            string cipherText = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, 
                SEED_ONE, KEY_ONE), "Texts should be different");
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFileSameSeed()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        #endregion

        #region Dif Seed Same Key Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStringDifSeedSameKeyTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo, 
                SEED_TWO, KEY_TWO), "Encrypted Texts Are Not Different");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void EncryptTxtFileDifSeedSameKeyTest()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1,encryptedFile1,fileToEncrypt2,encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrDifSeedSameKey()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.CompressEncrypt(TEST_MSG, ref keyTwo,
                SEED_TWO, KEY_ONE), "Encrypted Texts Are Not Different");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFileDifSeedSameKey()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        #endregion

        #region Different Seed  & Key Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStringDifSeedKeyTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo),
                "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void EncryptTxtFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1,encryptedFile1,fileToEncrypt2,encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrDifSeedKey()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.CompressEncrypt(TEST_MSG, ref keyTwo), "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFileDifSeedKey()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        #endregion

        #region Same Seed  & Key Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStringSameSeedKeyTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo, 
                SEED_ONE, KEY_ONE), "Strings encrypted to the same thing");
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void EncryptTxtFileSameSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrSameSeedKey()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo,
                SEED_ONE, KEY_ONE), "Strings encrypted to the same thing");
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressEncryptFileSameSeedKey()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        #endregion
        #endregion

        #region Decryption Tests

        #region Random Decrypt Tests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStringTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(cipherText);
            Assert.AreEqual(TEST_MSG, encryptor.Decrypt(cipherText, keyOne),
                "Decrypted text is not the same original text");
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptTxtFileTest()
        {
            string fileToEncrypt = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string encryptedFile = GetPath(TestContext.DataRow["encryptedFile"].ToString());
            string decryptedFile = GetPath(TestContext.DataRow["decryptedFile"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
            encryptor.Decrypt(encryptedFile, decryptedFile, keyOne);
            TestEncryptDecryption(decryptedFile,encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile),
                "Decrypted File Not Same Len As Orginal File");
        } 
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStr()
        {
            string cipherText = encryptor.CompressEncrypt(TEST_MSG, ref keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(cipherText);
            Assert.AreEqual(TEST_MSG, encryptor.DecompressDecrypt(cipherText,ref keyOne),
                "Decrypted text is not the same original text");
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFile()
        {
            string fileToEncrypt = GetPath(TestContext.DataRow["fileToEncrypt"].ToString());
            string encryptedFile = GetPath(TestContext.DataRow["encryptedFile"].ToString());
            string decryptedFile = GetPath(TestContext.DataRow["decryptedFile"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
            encryptor.DecompressDecrypt(encryptedFile, decryptedFile, keyOne);
            TestEncryptDecryption(decryptedFile, encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile),
                "Decrypted File Not Same Len As Orginal File");
        }
        #endregion

        #region Same Seed Dif KeysTests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStringSameSeedTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE, KEY_TWO);
            TestEncryptionAssert(cipherText1, cipherText2);
            checkSameSeedDifKey(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2,
                encryptor.Decrypt(cipherText1, keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStrSameSeed()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_ONE, KEY_TWO);
            TestEncryptionAssert(cipherText1, cipherText2);
            checkSameSeedDifKey(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1,
                ref keyOne), encryptor.DecompressDecrypt(cipherText1,ref keyOne));
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFileSameSeed()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_TWO);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        #endregion

        #region Dif Seed Tests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStringDifSeedTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_TWO, KEY_ONE);
            TestEncryptionAssert(cipherText1, cipherText2);
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, 
                keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptTxtFileDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        } 
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStrDifSeed()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_TWO, KEY_ONE);
            TestEncryptionAssert(cipherText1, cipherText2);
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1,
                ref keyOne), encryptor.DecompressDecrypt(cipherText1, ref keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFileDifSeed()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.DecompressDecrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.DecompressDecrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        #endregion

        #region Dif Seed & Key tests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStringDifSeedKeyTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo);
            TestEncryption(cipherText1);
            Assert.AreNotEqual(cipherText1, cipherText2, "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, 
                keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptTxtFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStrDifSeedKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo);
            TestEncryption(cipherText1);
            Assert.AreNotEqual(cipherText1, cipherText2, "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1,
                ref keyOne), encryptor.DecompressDecrypt(cipherText1, ref keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFileDifSeedKey()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, encryptedFile1);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.DecompressDecrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.DecompressDecrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        #endregion

        #region Same Seed & Key tests
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptStringSameKeySeedTest()
        {
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE,  KEY_ONE);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE, KEY_ONE);
            TestEncryptionAssert(cipherText1, cipherText2);
            Assert.AreNotEqual(cipherText1, cipherText2, "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, 
                keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void DecryptTxtFileSameKeySeedTest()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStrSameSeedKey()
        {
            string cipherText1 = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE, KEY_ONE);
            string cipherText2 = encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_ONE, KEY_ONE);
            TestEncryptionAssert(cipherText1, cipherText2);
            Assert.AreNotEqual(cipherText1, cipherText2, "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1,ref keyOne), encryptor.CompressEncrypt(cipherText1,ref keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "|DataDirectory|\\Tests.xml", "TwoFile", DataAccessMethod.Sequential)]
        public void CompressDecryptFileSameSeedKey()
        {
            string fileToEncrypt1 = GetPath(TestContext.DataRow["fileToEncryptOne"].ToString());
            string fileToEncrypt2 = GetPath(TestContext.DataRow["fileToEncryptTwo"].ToString());
            string encryptedFile1 = GetPath(TestContext.DataRow["encryptedFileOne"].ToString());
            string encryptedFile2 = GetPath(TestContext.DataRow["encryptedFileTwo"].ToString());
            string decryptedFile1 = GetPath(TestContext.DataRow["decryptedFileOne"].ToString());
            string decryptedFile2 = GetPath(TestContext.DataRow["decryptedFileTwo"].ToString());
            keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
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
            Assert.IsTrue(File.Exists(GetPath(TestContext.DataRow["fileToEncryptOne"].ToString())));
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
        static void checkDifSeed(KeyHolder keyOne, KeyHolder keyTwo)
        {
            checkPairNotNull(keyOne);
            checkPairNotNull(keyTwo);
            Assert.AreEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key)," keys are different from eachther");
            Assert.AreNotEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed),"seeds should be different from eachother");
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
        /// Function returns the whole path of a file
        /// </summary>
        /// <param name="path">//name of file to find whole path</param>
        /// <returns></returns>
        static string GetPath(string path)
        {
            return Directory.GetParent(path).FullName + path;
        }
        /// <summary>
        /// function tests that the given two files are not the same and have different lengths from each other
        /// </summary>
        /// <param name="decryptedFile"> unencrypted file</param>
        /// <param name="encryptedFile">encryptedFile</param>
        static void TestEncryptDecryption(string decryptedFile, string encryptedFile)
        {
            Assert.IsFalse(Encryption.FileCompare(decryptedFile, encryptedFile), "Decrypted file and Encrypted File are the same");
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
        /// <param name="saveDestination1">fist encrypted file</param>
        /// <param name="fileToEncrypt2">second file to encrypt</param>
        /// <param name="saveDestination2">second encrypted file</param>
        private static void SameSeedAndDifSeedTestHelper(string fileToEncrypt1, string saveDestination1, string fileToEncrypt2, string saveDestination2)
        {
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt1, fileToEncrypt2), "Files are not the same");
            Assert.IsFalse(Encryption.FileCompare(saveDestination1, saveDestination2), "Encrypted Files are the same");
        }
        /// <summary>
        /// function asserts that decrypted texts are the same and that they are different from their encrypted versions
        /// </summary>
        /// <param name="encryptedText1">encrypted string one</param>
        /// <param name="encryptedText2">encrypted string two</param>
        /// <param name="decryptdTxt1">decrytped string one</param>
        /// <param name="decryptdTxt2">decrypted string two</param>
        static void TestStrDecryptionSameSeed(string encryptedText1, string encryptedText2, string decryptdTxt1, string decryptdTxt2)
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
        private static void CheckFileDecryption(string fileToEncrypt1, string fileToEncrypt2, string encryptedFile1, string encryptedFile2, string decryptedFile1, string decryptedFile2)
        {
            Assert.IsFalse(Encryption.FileCompare(encryptedFile1, decryptedFile1), "Files are the same");
            Assert.IsFalse(Encryption.FileCompare(encryptedFile2, decryptedFile2), "Files are the same");
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt1, decryptedFile1), "Files are not the same");
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt2, decryptedFile2), "Files are not the same");
            Assert.IsTrue(Encryption.FileCompare(decryptedFile1, decryptedFile2), "Files are not the same");
        }
        /// <summary>
        /// function tests whether two strings are equal
        /// </summary>
        /// <param name="cipherText1">string to check</param>
        /// <param name="cipherText2"> second string to check</param>
        private static void TestEncryptionAssert(string cipherText1, string cipherText2)
        {
            TestEncryption(cipherText1);
            TestEncryption(cipherText2);
            Assert.AreNotEqual(cipherText1, cipherText2, "Msgs should not be equal");
        }
        #endregion
    }
}