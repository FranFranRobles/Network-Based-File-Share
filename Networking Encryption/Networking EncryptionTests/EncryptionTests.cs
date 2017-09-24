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
 * CodeMetrics: 59  61  1   12  641
 */
namespace Networking_Encryption.Tests
{

    [TestClass()]
    public class EncryptionTests
    {
        const string word = "this is a test";
        const string seedOne = "1";
        const string seedTwo = "2";

        #region FileCompare Tests
        [TestMethod()]
        public void FileCompareAreEqual()
        {
            string fileOne = GetPath(Files.TextToEncryptOne);
            string fileTwo = GetPath(Files.TextToEncryptTwo);
            Assert.IsTrue(Encryption.FileCompare(@fileOne, @fileTwo), "File Compare Funct");
        }
        [TestMethod()]
        public void FileCompareSameFile()
        {
            string fileOne = GetPath(Files.TextToEncryptOne);
            Assert.IsTrue(Encryption.FileCompare(fileOne, fileOne), "File Compare Funct");
        }
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
        public void EncryptStringTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keys = null;
            string cipherText = encryptor.EncryptStr(word, ref keys);
            checkPairNotNull(keys);
            TestEncryption(cipherText);
        }

        [TestMethod()]
        public void EncryptTxtFileTest()
        {
            string fileToEncrypt = GetPath(Files.TextToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedTextOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keys);
        }

        [TestMethod()]
        public void EncryptPdfTest()
        {
            string fileToEncrypt = GetPath(Files.PdfToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedPdfOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keys);
        }
        [TestMethod()]
        public void EncryptImgPngTest()
        {
            string fileToEncrypt = GetPath(Files.PngToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedPngOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keys);
        }
        [TestMethod()]
        public void EncryptImgGifTest()
        {
            string fileToEncrypt = GetPath(Files.GifToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedGifOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keys);
        }
        [TestMethod()]
        public void EncryptImgJpegTest()
        {
            string fileToEncrypt = GetPath(Files.JpegToEncryptOne);
            string saveDestination = GetPath(Files.EncryptedJpegOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            TestEncryptDecryption(fileToEncrypt, saveDestination);
            checkPairNotNull(keys);
        }
        [TestMethod()]
        public void CompressEncryptStr()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressEncryptFile()
        {
            Assert.Fail();
        }
        #endregion

        #region Same Seed Tests // need to change to have dif keys
        [TestMethod()]
        public void EncryptStringSameSeedTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText = encryptor.EncryptStr(word, ref keyOne, seedOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(word, ref keyTwo, seedOne));
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeed(keyOne, keyTwo);
        }

        [TestMethod()]
        public void EncryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPdfOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPdfTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPngOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPngTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedJpegOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedJpegTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedGifOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedGifTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void CompressEncryptStrSameSeed()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressEncryptFileSameSeed()
        {
            Assert.Fail();
        }
        #endregion

        #region Dif Seed Tests // need to change to have same key
        [TestMethod()]
        public void EncryptStringDifSeedTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText = encryptor.EncryptStr(word, ref keyOne, seedOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(word, ref keyTwo, seedTwo), "Encrypted Texts Are Not Different");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptTxtFileDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1,saveDestination1,fileToEncrypt2,saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptPdfDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPdfOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPdfTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgPngDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPngOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPngTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgJpegDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedJpegOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedJpegTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgGifDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedGifOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedGifTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void CompressEncryptStrDifSeed()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressEncryptFileDifSeed()
        {
            Assert.Fail();
        }
        #endregion

        #region Different Seed  & Key Tests
        [TestMethod()]
        public void EncryptStringDifSeedKeyTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText = encryptor.EncryptStr(word, ref keyOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(word, ref keyTwo), "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptTxtFileDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1,saveDestination1,fileToEncrypt2,saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptPdfDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPdfOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPdfTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgPngDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPngOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPngTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgJpegDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedJpegOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedJpegTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgGifDifSeedKeyTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedGifOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedGifTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void CompressEncryptStrDifSeedKey()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressEncryptFileDifSeedKey()
        {
            Assert.Fail();
        }
        #endregion

        #region Same Seed  & Key Tests // need to implement the new tests
        [TestMethod()]
        public void EncryptStringSameSeedKeyTest()
        {
            Assert.Fail();
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText = encryptor.EncryptStr(word, ref keyOne);
            TestEncryption(cipherText);
            Assert.AreNotEqual(cipherText, encryptor.EncryptStr(word, ref keyTwo), "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptTxtFileSameSeedKeyTest()
        {
            Assert.Fail();
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedTextOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedTextTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptPdfSameSeedKeyTest()
        {
            Assert.Fail();
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPdfOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPdfTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgPngSameSeedKeyTest()
        {
            Assert.Fail();
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedPngOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedPngTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgJpegSameSeedKeyTest()
        {
            Assert.Fail();
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedJpegOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedJpegTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgGifSameSeedKeyTest()
        {
            Assert.Fail();
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string saveDestination1 = GetPath(Files.EncryptedGifOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string saveDestination2 = GetPath(Files.EncryptedGifTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            TestEncryptDecryption(fileToEncrypt1, saveDestination1);
            TestEncryptDecryption(fileToEncrypt2, saveDestination2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, saveDestination1, fileToEncrypt2, saveDestination2);
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void CompressEncryptStrSameSeedKey()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressEncryptFileSameSeedKey()
        {
            Assert.Fail();
        }
        #endregion
        #endregion

        #region Decryption Tests

        #region Random Decrypt Tests
        [TestMethod()]
        public void DecryptStringTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keys = null;
            string cipherText = encryptor.EncryptStr(word, ref keys);
            checkPairNotNull(keys);
            TestEncryption(cipherText);
            Assert.AreEqual(word, encryptor.Decrypt(cipherText, keys), "Decrypted text is not the same original text");
            checkPairNotNull(keys);
        }
        [TestMethod()]
        public void DecryptTxtFileTest()
        {
            string fileToEncrypt = GetPath(Files.TextToEncryptOne);
            string encryptedFile = GetPath(Files.EncryptedTextOne);
            string decryptedFile = GetPath(Files.DecryptedTextOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keys);
            encryptor.Decrypt(encryptedFile, decryptedFile, keys);
            TestEncryptDecryption(decryptedFile,encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile), "Decrypted File Not Same Len As Orginal File");
        }
        [TestMethod()]
        public void DecryptPdfTest()
        {
            string fileToEncrypt = GetPath(Files.PdfToEncryptOne);
            string encryptedFile = GetPath(Files.EncryptedPdfOne);
            string decryptedFile = GetPath(Files.DecryptedPdfOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keys);
            encryptor.Decrypt(encryptedFile, decryptedFile, keys);
            TestEncryptDecryption(decryptedFile, encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile), "Decrypted File Not Same Len As Orginal File");
        }
        [TestMethod()]
        public void DecryptImgPngTest()
        {
            string fileToEncrypt = GetPath(Files.PngToEncryptOne);
            string encryptedFile = GetPath(Files.EncryptedPngOne);
            string decryptedFile = GetPath(Files.DecryptedPngOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, encryptedFile);
            TestEncryptDecryption(fileToEncrypt, encryptedFile);
            checkPairNotNull(keys);
            encryptor.Decrypt(encryptedFile, decryptedFile, keys);
            TestEncryptDecryption(decryptedFile, encryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, decryptedFile), "Decrypted File Not Same Len As Orginal File");
        }
        [TestMethod()]
        public void DecryptImgGifTest()
        {
            string fileToEncrypt = GetPath(Files.GifToEncryptOne);
            string EncryptedFile = GetPath(Files.EncryptedGifOne);
            string DecryptedFile = Directory.GetParent(Files.DecryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifOne);
            Encryption encryptor = new Encryption();
            KeyHolder keys = encryptor.Encrypt(fileToEncrypt, EncryptedFile);
            TestEncryptDecryption(fileToEncrypt, EncryptedFile);
            checkPairNotNull(keys);
            encryptor.Decrypt(EncryptedFile, DecryptedFile, keys);
            TestEncryptDecryption(DecryptedFile, EncryptedFile);
            Assert.IsTrue(Encryption.FileCompare(fileToEncrypt, DecryptedFile), "Decrypted File Not Same Len As Orginal File");
        }
        [TestMethod()]
        public void CompressDecryptStr()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressDecryptFile()
        {
            Assert.Fail();
        }
        #endregion

        #region Same Seed Tests // need to change to have dif keys
        [TestMethod()]
        public void DecryptStringSameSeedTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText1 = encryptor.EncryptStr(word, ref keyOne, seedOne);
            string cipherText2 = encryptor.EncryptStr(word, ref keyTwo, seedOne);
            TestEncryption(cipherText1);
            Assert.AreNotEqual(cipherText1, encryptor.EncryptStr(word, ref keyTwo, seedOne));
            checkSameSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPdfOne);
            string encryptedFile2 = GetPath(Files.EncryptedPdfTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPdfOne);
            string decryptedFile2 = GetPath(Files.DecryptedPdfTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPngOne);
            string encryptedFile2 = GetPath(Files.EncryptedPngTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPngOne);
            string decryptedFile2 = GetPath(Files.DecryptedPngTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedJpegOne);
            string encryptedFile2 = GetPath(Files.EncryptedJpegTwo);
            string decryptedFile1 = GetPath(Files.DecryptedJpegOne);
            string decryptedFile2 = GetPath(Files.DecryptedJpegTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedGifOne);
            string encryptedFile2 = GetPath(Files.EncryptedGifTwo);
            string decryptedFile1 = GetPath(Files.DecryptedGifOne);
            string decryptedFile2 = GetPath(Files.DecryptedGifTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedOne);
            TestEncryptDecryption(fileToEncrypt1, encryptedFile1);
            TestEncryptDecryption(fileToEncrypt2, encryptedFile2);
            SameSeedAndDifSeedTestHelper(fileToEncrypt1, encryptedFile1, fileToEncrypt2, encryptedFile2);
            checkSameSeed(keyOne, keyTwo);
            encryptor.Decrypt(encryptedFile1, decryptedFile1, keyOne);
            encryptor.Decrypt(encryptedFile2, decryptedFile2, keyTwo);
            CheckFileDecryption(fileToEncrypt1, fileToEncrypt2, encryptedFile1, encryptedFile2, decryptedFile1, decryptedFile2);
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void CompressDecryptStrSameSeed()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressDecryptFileSameSeed()
        {
            Assert.Fail();
        }
        #endregion

        #region Dif Seed Tests // need to change to have same key
        [TestMethod()]
        public void DecryptStringDifSeedTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText1 = encryptor.EncryptStr(word, ref keyOne, seedOne);
            string cipherText2 = encryptor.EncryptStr(word, ref keyTwo, seedTwo);
            TestEncryption(cipherText1);
            TestEncryption(cipherText2);
            Assert.AreNotEqual(cipherText1, cipherText2, "Encrypted Texts Are Not Different");
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptTxtFileDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.TextToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.TextToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedTextOne);
            string encryptedFile2 = GetPath(Files.EncryptedTextTwo);
            string decryptedFile1 = GetPath(Files.DecryptedTextOne);
            string decryptedFile2 = GetPath(Files.DecryptedTextTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedTwo);
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
        public void DecryptPdfDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PdfToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PdfToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPdfOne);
            string encryptedFile2 = GetPath(Files.EncryptedPdfTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPdfOne);
            string decryptedFile2 = GetPath(Files.DecryptedPdfTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedTwo);
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
        public void DecryptImgPngDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.PngToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.PngToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedPngOne);
            string encryptedFile2 = GetPath(Files.EncryptedPngTwo);
            string decryptedFile1 = GetPath(Files.DecryptedPngOne);
            string decryptedFile2 = GetPath(Files.DecryptedPngTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedTwo);
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
        public void DecryptImgJpegDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.JpegToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.JpegToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedJpegOne);
            string encryptedFile2 = GetPath(Files.EncryptedJpegTwo);
            string decryptedFile1 = GetPath(Files.DecryptedJpegOne);
            string decryptedFile2 = GetPath(Files.DecryptedJpegTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedTwo);
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
        public void DecryptImgGifDifSeedTest()
        {
            string fileToEncrypt1 = GetPath(Files.GifToEncryptOne);
            string fileToEncrypt2 = GetPath(Files.GifToEncryptTwo);
            string encryptedFile1 = GetPath(Files.EncryptedGifOne);
            string encryptedFile2 = GetPath(Files.EncryptedGifTwo);
            string decryptedFile1 = GetPath(Files.DecryptedGifOne);
            string decryptedFile2 = GetPath(Files.DecryptedGifTwo);
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = encryptor.Encrypt(fileToEncrypt1, encryptedFile1, seedOne);
            KeyHolder keyTwo = encryptor.Encrypt(fileToEncrypt2, encryptedFile2, seedTwo);
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
        public void CompressDecryptStrDifSeed()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressDecryptFileDifSeed()
        {
            Assert.Fail();
        }
        #endregion

        #region Dif Seed & Key tests
        public void DecryptStringDifSeedKeyTest()
        {
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText1 = encryptor.EncryptStr(word, ref keyOne);
            string cipherText2 = encryptor.EncryptStr(word, ref keyTwo);
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
        public void DecryptStringSameKeySeedTest()
        {
            Assert.Fail();
            Encryption encryptor = new Encryption();
            KeyHolder keyOne = null;
            KeyHolder keyTwo = null;
            string cipherText1 = encryptor.EncryptStr(word, ref keyOne);
            string cipherText2 = encryptor.EncryptStr(word, ref keyTwo);
            TestEncryption(cipherText1);
            Assert.AreNotEqual(cipherText1, cipherText2, "Strings encrypted to the same thing");
            checkDifSeed(keyOne, keyTwo);
            TestStrDecryptionSameSeed(cipherText1, cipherText2, encryptor.Decrypt(cipherText1, keyOne), encryptor.Decrypt(cipherText1, keyOne));
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptTxtFileSameKeySeedTest()
        {
            Assert.Fail();
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
        public void DecryptPdfSameKeySeedTest()
        {
            Assert.Fail();
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
        public void DecryptImgPngSameKeySeedTest()
        {
            Assert.Fail();
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
        public void DecryptImgJpegSameKeySeedTest()
        {
            Assert.Fail();
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
        public void DecryptImgGifSameKeySeedTest()
        {
            Assert.Fail();
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
        public void CompressDecryptStrSameSeedKey()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void CompressDecryptFileSameSeedKey()
        {
            Assert.Fail();
        }
        #endregion
        #endregion

        #region Resource Tests
        [TestMethod()]
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
        static void checkSameSeed(KeyHolder keyOne, KeyHolder keyTwo)
        {
            checkPairNotNull(keyOne);
            checkPairNotNull(keyTwo);
            Assert.AreNotEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key));
            Assert.AreEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed));
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
            Assert.AreNotEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key));
            Assert.AreNotEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed));
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
        ///         /// function tests that the given two files are not the same and have different lengths from each other
        /// </summary>
        /// <param name="cipherText">Encrypted Text</param>
        private static void TestEncryption(string cipherText)
        {
            Assert.AreNotEqual(word, cipherText, "Text Did Not Encrypt");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Data was not Salted");
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
            Assert.AreEqual(word, decryptdTxt1, "Text Not The Same");
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
        #endregion
    }
}