using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * CodeMetrics: 74  32  1   16  56
 */
namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class KeyHolderTest
    {
        const string KEY_HOLDER_CAT = "KeyHolder Tests";
        const string EXCEPTION_TXT = "Exception";
        #region Test Constants
        const int KeyLen = 32;
        const int SeedLen = 16;
        const byte TestNum = 55;
        const int invalidNum = 55;
        const int strTestNum = 132;

        #endregion
        #region Intialize Tests
        public KeyHolder test = null;
        [TestInitialize]
        public void IntializeTests()
        {
            test = new KeyHolder();
        }
        #endregion
        #region CTOR Unit Tests
        [TestMethod()]
        [TestCategory(KEY_HOLDER_CAT)]
        public void KeyHolderDefaultCTOR()
        {
            Assert.IsNull(test.Key);
            Assert.IsNull(test.Seed);
        }
        [TestMethod()]
        [TestCategory(KEY_HOLDER_CAT)]
        public void KeyHolderByteCTOR()
        {
            byte[] key = Enumerable.Repeat(TestNum, KeyLen).ToArray();
            byte[] seed = Enumerable.Repeat((byte)(TestNum + 5), SeedLen).ToArray();
            test = new KeyHolder(key, seed);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        [TestCategory(KEY_HOLDER_CAT)]
        public void KeyHolderStringCTOR()
        {
            string key = string.Join("", Enumerable.Repeat("123", KeyLen).ToArray());
            string seed = string.Join("", Enumerable.Repeat("136", SeedLen).ToArray());
            test = new KeyHolder(key, seed);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        #endregion

        #region SetFunctions Unit Tests
        [TestMethod()]
        [TestCategory(KEY_HOLDER_CAT)]
        public void setKeyByteTest()
        {
            byte[] key = Enumerable.Repeat(TestNum, KeyLen).ToArray();
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        [TestMethod()]
        [TestCategory(KEY_HOLDER_CAT)]
        public void setKeyStringByteTest()
        {
            string key = string.Join("", Enumerable.Repeat(strTestNum, KeyLen).ToArray());
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        [TestMethod()]
        [TestCategory(KEY_HOLDER_CAT)]
        public void setSeedByteTest()
        {
            byte[] seed = Enumerable.Repeat(TestNum, SeedLen).ToArray();
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        [TestCategory(KEY_HOLDER_CAT)]
        public void setSeedStringTest()
        {
            string seed = string.Join("", Enumerable.Repeat(strTestNum, SeedLen).ToArray());
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        #endregion

        #region Exception is Thrown Unit Tests

        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void setSeedStringInvalidLenExceptionTest()
        {
            test.setSeed(string.Join("", Enumerable.Repeat((byte)155, 10).ToArray()));
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(OutOfDomainException))]
        public void setSeedStringOutOfDomainExceptionTest()
        {
            test.setSeed(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray()));
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void setSeedByteInvalidLenExceptionTest()
        {
            test.setSeed(Enumerable.Repeat(TestNum, (SeedLen - 5)).ToArray());
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void setKeyStringInvalidLenExceptionTest()
        {
            test.setKey(string.Join("", Enumerable.Repeat((byte)155, 10).ToArray()));
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(OutOfDomainException))]
        public void setKeyStringOutOfDomianExceptionTest()
        {
            test.setKey(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray()));
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void setKeyByteInvalidLenExceptionTest()
        {
            test.setKey(Enumerable.Repeat(TestNum, (KeyLen - 5)).ToArray());
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(OutOfDomainException))]
        public void parseStrIsKeyOutOfDomainTest()
        {
            KeyHolder.parseString(string.Join("", Enumerable.Repeat(invalidNum, KeyLen).ToArray()), true);
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void parseStrIsKeyInvalidLenTest()
        {
            KeyHolder.parseString(string.Join("", Enumerable.Repeat((byte)155, (KeyLen - 3)).ToArray()), true);
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(OutOfDomainException))]
        public void parseStrNotKeyOutOfDomainTest()
        {
            KeyHolder.parseString(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray()), false);
        }
        [TestMethod()]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void parseStrNotKeyInvalidLenTest()
        {
            KeyHolder.parseString(string.Join("", Enumerable.Repeat((byte)155, (SeedLen - 5)).ToArray()), false);
        }
        #endregion
    }
}