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
 * 
 * // need to add priority to tests 
 * -- categories 
 */
namespace Networking_Encryption.Tests
{

    [TestClass()]
    public class EncryptionTests
    { // Test Constants
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
        public void FileCompareAreEqual()
        {
            string fileOne = GetPath(Files.TextToEncryptOne);
            string fileTwo = GetPath(Files.TextToEncryptTwo);
            Assert.IsTrue(Encryption.FileCompare(@fileOne, @fileTwo), "File Compare Funct");
        }
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        public void FileCompareSameFile()
        {
            string fileOne = GetPath(Files.TextToEncryptOne);
            Assert.IsTrue(Encryption.FileCompare(fileOne, fileOne), "File Compare Funct");
        }
        [TestMethod()]
        [TestCategory(FILE_COMPARE)]
        public void FileCompareNotEqual()
        {
            string fileOne = GetPath(Files.TextToEncryptOne);
            string fileTwo = GetPath(Files.DecryptedTextOne);
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
        public void EncryptTxtFileTest()
        {
            string fileToEncrypt = GetPath(Files.TextToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedTextOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptPdfTest()
        {
            string fileToEncrypt = GetPath(Files.PdfToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedPdfOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgPngTest()
        {
            string fileToEncrypt = GetPath(Files.PngToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedPngOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgGifTest()
        {
            string fileToEncrypt = GetPath(Files.GifToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedGifOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgJpegTest()
        {
            string fileToEncrypt = GetPath(Files.JpegToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedJpegOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
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
        public void CompressEncryptFile()
        {
            string fileToEncrypt = GetPath(Files.TextToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedTextOne);
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
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE), "Texts should be different");
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPdfOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPdfTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPngOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPngTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedJpegOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedJpegTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedGifOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedGifTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrSameSeed()
        {
            string cipherText = encryptor.CompressEncrypt(TEST_MSG, ref keyOne, SEED_ONE);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_ONE), "Texts should be different");
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptFileSameSeed()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, saveDestination1, SEED_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, saveDestination2, SEED_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        #endregion

        #region Dif Seed Same Key Tests
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptStringDifSeedSameKeyTest()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_TWO, KEY_ONE), "Encrypted Texts Are Not Different");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptTxtFileDifSeedSameKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1,saveDestination1,fileToEncrypt2,saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptPdfDifSeedSameKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPdfOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPdfTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgPngDifSeedSameKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPngOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPngTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgJpegDifSeedSameKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedJpegOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedJpegTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgGifDifSeedSameKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedGifOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedGifTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrDifSeedSameKey()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne, SEED_ONE);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.CompressEncrypt(TEST_MSG, ref keyTwo, SEED_TWO, KEY_ONE), "Encrypted Texts Are Not Different");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptFileDifSeedSameKey()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_TWO, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
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
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo), "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptTxtFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1,saveDestination1,fileToEncrypt2,saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptPdfDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPdfOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPdfTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgPngDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPngOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPngTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgJpegDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedJpegOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedJpegTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgGifDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedGifOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedGifTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
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
        public void CompressEncryptFileDifSeedKey()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, saveDestination1);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
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
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE, KEY_ONE), "Strings encrypted to the same thing");
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptTxtFileSameSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptPdfSameSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPdfOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPdfTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgPngSameSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPngOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPngTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgJpegSameSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedJpegOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedJpegTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void EncryptImgGifSameSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedGifOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedGifTwo);
            keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptStrSameSeedKey()
        {
            string cipherText = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(TEST_MSG, ref keyTwo, SEED_ONE, KEY_ONE), "Strings encrypted to the same thing");
            checkSameSeedKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(ENCRYPT_TESTS)]
        public void CompressEncryptFileSameSeedKey()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            keyOne = encryptor.CompressEncrypt(fileToEncrypt1, saveDestination1, SEED_ONE, KEY_ONE);
            keyTwo = encryptor.CompressEncrypt(fileToEncrypt2, saveDestination2, SEED_ONE, KEY_ONE);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
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
            Assert.AreEqual(TEST_MSG, encryptor.Decrypt(cipherText, keyOne), "Decrypted text is not the same original text");
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptTxtFileTest()
        {
            string fileToEncrypt = GetPath(Files.TextToEncryptOne);
            string encryptedFile = GetPath(Files.EncryptedTextOne);
            string decryptedFile = GetPath(Files.DecryptedTextOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
            encryptor.Decrypt(encryptedFile, decryptedFile, keyOne);
            TestEncryptDecryption(decryptedFile,encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile), "Decrypted File Not Same Len As Orginal File");
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptPdfTest()
        {
            string fileToEncrypt = GetPath(Files.PdfToEncryptOne);
            string encryptedFile = GetPath(Files.EncryptedPdfOne);
            string decryptedFile = GetPath(Files.DecryptedPdfOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
            encryptor.Decrypt(encryptedFile, decryptedFile, keyOne);
            TestEncryptDecryption(decryptedFile, encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile), "Decrypted File Not Same Len As Orginal File");
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptImgPngTest()
        {
            string fileToEncrypt = GetPath(Files.PngToEncryptOne);
            string encryptedFile = GetPath(Files.EncryptedPngOne);
            string decryptedFile = GetPath(Files.DecryptedPngOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
            encryptor.Decrypt(encryptedFile, decryptedFile, keyOne);
            TestEncryptDecryption(decryptedFile, encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile), "Decrypted File Not Same Len As Orginal File");
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptImgGifTest()
        {
            string fileToEncrypt = GetPath(Files.GifToEncryptOne);
            string EncryptedFile = GetPath(Files.EncryptedGifOne);
            string DecryptedFile = Directory.GetParent(Files.DecryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifOne);
            keyOne = encryptor.Encrypt(fileToEncrypt, EncryptedFile);
            TestEncryptDecryption(fileToEncrypt, EncryptedFile);
            checkPairNotNull(keyOne);
            encryptor.Decrypt(EncryptedFile, DecryptedFile, keyOne);
            TestEncryptDecryption(DecryptedFile, EncryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, DecryptedFile), "Decrypted File Not Same Len As Orginal File");
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptStr()
        {
            string cipherText = encryptor.CompressEncrypt(TEST_MSG, ref keyOne);
            checkPairNotNull(keyOne);
            TestEncryption(cipherText);
            Assert.AreEqual(TEST_MSG, encryptor.DecompressDecrypt(cipherText,ref keyOne), "Decrypted text is not the same original text");
            checkPairNotNull(keyOne);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptFile()
        {
            string fileToEncrypt = GetPath(Files.TextToEncryptOne);
            string encryptedFile = GetPath(Files.EncryptedTextOne);
            string decryptedFile = GetPath(Files.DecryptedTextOne);
            keyOne = encryptor.CompressEncrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keyOne);
            encryptor.DecompressDecrypt(encryptedFile, decryptedFile, keyOne);
            TestEncryptDecryption(decryptedFile, encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile), "Decrypted File Not Same Len As Orginal File");
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
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
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
        public void DecryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPdfOne);
            string encryptedFile2 = GetPath(Files.EncryptedPdfTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPdfOne);
            string decryptedFile2 = GetPath(Files.DecryptedPdfTwo);
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
        public void DecryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPngOne);
            string encryptedFile2 = GetPath(Files.EncryptedPngTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPngOne);
            string decryptedFile2 = GetPath(Files.DecryptedPngTwo);
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
        public void DecryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedJpegOne);
            string encryptedFile2 = GetPath(Files.EncryptedJpegTwo);
            string decryptedFile1 = GetPath(Files.DecryptedJpegOne);
            string decryptedFile2 = GetPath(Files.DecryptedJpegTwo);
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
        public void DecryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedGifOne);
            string encryptedFile2 = GetPath(Files.EncryptedGifTwo);
            string decryptedFile1 = GetPath(Files.DecryptedGifOne);
            string decryptedFile2 = GetPath(Files.DecryptedGifTwo);
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
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1,ref keyOne), encryptor.DecompressDecrypt(cipherText1,ref keyOne));
            checkSameSeedDifKey(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptFileSameSeed()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
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
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptTxtFileDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
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
        public void DecryptPdfDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPdfOne);
            string encryptedFile2 = GetPath(Files.EncryptedPdfTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPdfOne);
            string decryptedFile2 = GetPath(Files.DecryptedPdfTwo);
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
        public void DecryptImgPngDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPngOne);
            string encryptedFile2 = GetPath(Files.EncryptedPngTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPngOne);
            string decryptedFile2 = GetPath(Files.DecryptedPngTwo);
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
        public void DecryptImgJpegDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedJpegOne);
            string encryptedFile2 = GetPath(Files.EncryptedJpegTwo);
            string decryptedFile1 = GetPath(Files.DecryptedJpegOne);
            string decryptedFile2 = GetPath(Files.DecryptedJpegTwo);
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
        public void DecryptImgGifDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedGifOne);
            string encryptedFile2 = GetPath(Files.EncryptedGifTwo);
            string decryptedFile1 = GetPath(Files.DecryptedGifOne);
            string decryptedFile2 = GetPath(Files.DecryptedGifTwo);
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
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.DecompressDecrypt(cipherText1, ref keyOne), encryptor.DecompressDecrypt(cipherText1,ref keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void CompressDecryptFileDifSeed()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
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
        public void DecryptStringDifSeedKeyTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText1 = encryptor.EncryptStr(TEST_MSG, ref keyOne);
            string cipherText2 = encryptor.EncryptStr(TEST_MSG, ref keyTwo);
            TestEncryption(cipherText1);
            Assert.AreNotEqual(cipherText1, cipherText2, "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptTxtFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptPdfDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPdfOne);
            string encryptedFile2 = GetPath(Files.EncryptedPdfTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPdfOne);
            string decryptedFile2 = GetPath(Files.DecryptedPdfTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgPngDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPngOne);
            string encryptedFile2 = GetPath(Files.EncryptedPngTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPngOne);
            string decryptedFile2 = GetPath(Files.DecryptedPngTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgJpegDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedJpegOne);
            string encryptedFile2 = GetPath(Files.EncryptedJpegTwo);
            string decryptedFile1 = GetPath(Files.DecryptedJpegOne);
            string decryptedFile2 = GetPath(Files.DecryptedJpegTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgGifDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedGifOne);
            string encryptedFile2 = GetPath(Files.EncryptedGifTwo);
            string decryptedFile1 = GetPath(Files.DecryptedGifOne);
            string decryptedFile2 = GetPath(Files.DecryptedGifTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void CompressDecryptStrDifSeedKey()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressDecryptFileDifSeedKey()
        {
            Assert.Fail();
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
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptTxtFileSameKeySeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
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
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptPdfSameKeySeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPdfOne);
            string encryptedFile2 = GetPath(Files.EncryptedPdfTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPdfOne);
            string decryptedFile2 = GetPath(Files.DecryptedPdfTwo);
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
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptImgPngSameKeySeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPngOne);
            string encryptedFile2 = GetPath(Files.EncryptedPngTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPngOne);
            string decryptedFile2 = GetPath(Files.DecryptedPngTwo);
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
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptImgJpegSameKeySeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedJpegOne);
            string encryptedFile2 = GetPath(Files.EncryptedJpegTwo);
            string decryptedFile1 = GetPath(Files.DecryptedJpegOne);
            string decryptedFile2 = GetPath(Files.DecryptedJpegTwo);
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
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        [TestCategory(DECRYPT_TESTS)]
        public void DecryptImgGifSameKeySeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedGifOne);
            string encryptedFile2 = GetPath(Files.EncryptedGifTwo);
            string decryptedFile1 = GetPath(Files.DecryptedGifOne);
            string decryptedFile2 = GetPath(Files.DecryptedGifTwo);
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
        public void CompressDecryptFileSameSeedKey()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
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
        public void testResourceLocations()
        {
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedGifOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedGifTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedJpegOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedJpegTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedPdfOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedPdfTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedPngOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedPngTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedTextOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.DecryptedTextTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedGifOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedGifTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedJpegOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedJpegTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedPdfOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedPdfTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedPngOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedPngTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedTextOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.EncryptedTextTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.GifToEncryptOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.GifToEncryptTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.JpegToEncryptOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.JpegToEncryptTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.PdfToEncryptOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.PdfToEncryptTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.PngToEncryptOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.PngToEncryptTwo)));
            Assert.IsTrue(File.Exists(GetPath(Files.TextToEncryptOne)));
            Assert.IsTrue(File.Exists(GetPath(Files.TextToEncryptTwo)));
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