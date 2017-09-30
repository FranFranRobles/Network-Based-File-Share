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
        #endregion

        #region CTOR Unit Tests
        [TestMethod()]
        [Priority(0)]
        [TestCategory(KEY_HOLDER_CAT)]
        public void KeyHolderDefaultCTOR()
        {
            KeyHolder test = new KeyHolder();
            Assert.IsNull(test.Key);
            Assert.IsNull(test.Seed);
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(KEY_HOLDER_CAT)]
        public void KeyHolderByteCTOR()
        {
            byte[] key = Enumerable.Repeat(TestNum, KeyLen).ToArray();
            byte[] seed = Enumerable.Repeat((byte)(TestNum + 5), SeedLen).ToArray();
            KeyHolder test = new KeyHolder(key, seed);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(KEY_HOLDER_CAT)]
        public void KeyHolderStringCTOR()
        {
            string key = string.Join("", Enumerable.Repeat("123", KeyLen).ToArray());
            string seed = string.Join("", Enumerable.Repeat("136", SeedLen).ToArray());
            KeyHolder test = new KeyHolder(key, seed);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        #endregion

        #region SetFunctions Unit Tests
        [TestMethod()]
        [Priority(0)]
        [TestCategory(KEY_HOLDER_CAT)]
        public void setKeyByteTest()
        {
            KeyHolder test = new KeyHolder();
            byte[] key = Enumerable.Repeat(TestNum, KeyLen).ToArray();
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(KEY_HOLDER_CAT)]
        public void setKeyStringByteTest()
        {
            KeyHolder test = new KeyHolder();
            string key = string.Join("", Enumerable.Repeat(TestNum, KeyLen).ToArray());
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(KEY_HOLDER_CAT)]
        public void setSeedByteTest()
        {
            KeyHolder test = new KeyHolder();
            byte[] seed = Enumerable.Repeat(TestNum, SeedLen).ToArray();
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(KEY_HOLDER_CAT)]
        public void setSeedStringTest()
        {
            KeyHolder test = new KeyHolder();
            string seed = string.Join("", Enumerable.Repeat(TestNum, SeedLen).ToArray());
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Key));
        }
        #endregion

        #region Exception is Thrown Unit Tests

        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void setSeedStringInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            test.setSeed(string.Join("", Enumerable.Repeat((byte)155, 10).ToArray()));
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(OutOfDomainException))]
        public void setSeedStringOutOfDomainExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            test.setSeed(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray()));
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void setSeedByteInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            test.setSeed(Enumerable.Repeat(TestNum, (SeedLen - 5)).ToArray());
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void setKeyStringInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            test.setKey(string.Join("", Enumerable.Repeat((byte)155, 10).ToArray()));
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(OutOfDomainException))]
        public void setKeyStringOutOfDomianExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            test.setKey(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray()));
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void setKeyByteInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            test.setKey(Enumerable.Repeat(TestNum, (KeyLen - 5)).ToArray());
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(OutOfDomainException))]
        public void parseStrIsKeyOutOfDomainTest()
        {
            KeyHolder.parseString(string.Join("", Enumerable.Repeat(invalidNum, KeyLen).ToArray()), true);
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(InvalidLengthException))]
        public void parseStrIsKeyInvalidLenTest()
        {
            KeyHolder.parseString(string.Join("", Enumerable.Repeat((byte)155, (KeyLen - 3)).ToArray()), true);
        }
        [TestMethod()]
        [Priority(0)]
        [TestCategory(EXCEPTION_TXT)]
        [TestCategory(KEY_HOLDER_CAT)]
        [ExpectedException(typeof(OutOfDomainException))]
        public void parseStrNotKeyOutOfDomainTest()
        {
            KeyHolder.parseString(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray()), false);
        }
        [TestMethod()]
        [Priority(0)]
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