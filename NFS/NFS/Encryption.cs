﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
/*
 * CodeMetrics: 57  40  1   11  123
 */
namespace Networking_Encryption
{
    /// <summary>
    /// class encrypts and decrypts given data
    /// </summary>
    public class Encryption
    {

        #region Aes Encryption Class
        class AesEncryption
        {
            //class constants:
            private const int AesKeySize = 32;
            private const int AesSeedSize = 16;

            //class atributes
            private byte[] key = null;
            private byte[] seed = null;

            #region Get/Set Functs
            public byte[] Key
            {
                get { return key; }
                set { key = value; }
            }
            public byte[] Seed
            {
                get { return seed; }
                set { seed = value; }
            }
            #endregion

            #region Key / Seed Functions
            /// <summary>
            /// Generates a random key for encryption
            /// </summary>
            public void GenKey()
            {
                if (key == null)
                {
                    key = new byte[AesKeySize];
                    using (RandomNumberGenerator ranGen = RandomNumberGenerator.Create())
                    {
                        ranGen.GetBytes(key);
                    }
                }
            }
            /// <summary>
            /// generates a random seed for the encryption
            /// </summary>
            public void GenSeed()
            {
                if (seed == null)
                {
                    seed = new byte[AesSeedSize];
                    using (RandomNumberGenerator ranGen = RandomNumberGenerator.Create())
                    {
                        ranGen.GetBytes(seed);
                    }
                }
            }
            /// <summary>
            /// function eliminates the stored values of the key & seed
            /// </summary>
            public void FlushKeys()
            {
                key = seed = null;
            }
            #endregion

            #region Encryption Functions
            /// <summary>
            /// Function encrypts the given string
            /// <para>Returns an encrypted string</para>
            /// </summary>
            /// <param name="strToEncrypt">String to encrypt</param>
            /// <returns>Returns an encrypted string</returns>
            public string Encrypt(string strToEncrypt)
            {
                string encryptedStr = "";
                byte[] strAsBytes = Encoding.ASCII.GetBytes(strToEncrypt);
                byte[] encryptedBytes = null;

                GenKey();
                GenSeed();
                using (MemoryStream memStrm = new MemoryStream(strAsBytes.Length))
                {
                    using (Aes aes = Aes.Create())
                    {
                        using (ICryptoTransform cryptTransfrm = aes.CreateEncryptor(key, seed))
                        {
                            using (CryptoStream cryptoStrm = new CryptoStream(memStrm, cryptTransfrm, CryptoStreamMode.Write))
                            {
                                cryptoStrm.Write(strAsBytes, 0, strAsBytes.Length);
                                cryptoStrm.FlushFinalBlock();
                                encryptedBytes = memStrm.ToArray();
                            }
                        }
                    }
                }
                string len = strToEncrypt.Length.ToString("X");//converts Int to Hex
                len += "X";//parse const
                encryptedStr += Convert.ToBase64String(encryptedBytes); // encrypted Str
                return len + encryptedStr;
            }
            /// <summary>
            /// encrypts given file and saves it to a selected destination
            /// </summary>
            /// <param name="inputStream">file to encrypt</param>
            /// <param name="outputStream">file to decrypt</param>
            public void Encrypt(FileStream inputStream, FileStream outputStream)
            {
                const int BUFFER_SIZE = 4096;// size of block of data to read
                byte[] buffer = new byte[BUFFER_SIZE];// array to place read data


                GenKey();
                GenSeed();
                using (Aes cryptoAlgo = Aes.Create())
                {
                    using (ICryptoTransform encryptor = cryptoAlgo.CreateEncryptor(key, seed))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                        {
                            int count;
                            while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0) // not end of the stream
                            {
                                cryptoStream.Write(buffer, 0, count);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Decryption Functions
            /// <summary>
            /// Function decrypts a given Stream and saves it to an output stream
            /// </summary>
            /// <param name="inputStream">Stream to decrypt</param>
            /// <param name="outputStream">Stream to save decryption</param>
            public void Decrypt(FileStream inputStream, FileStream outputStream)
            {
                const int BUFFER_SIZE = 4096;
                byte[] buffer = new byte[BUFFER_SIZE];
                using (Aes cryptoAlgo = Aes.Create())
                {
                    using (ICryptoTransform decryptor = cryptoAlgo.CreateDecryptor(key, seed))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                        {
                            int count;
                            while ((count = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                outputStream.Write(buffer, 0, count);
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Decrypts given string 
            /// <para>Returns a decrypted string</para>
            /// </summary>
            /// <param name="encryptedStr">String to decrypt</param>
            /// <returns>Returns a decrypted string</returns>
            public string Decrypt(string encryptedStr)
            {
                int len = GetLen(ref encryptedStr);
                byte[] encrypStrAsBytes = Convert.FromBase64String(encryptedStr);
                byte[] orginalText = new Byte[encrypStrAsBytes.Length];

                using (MemoryStream memStrm = new MemoryStream(encrypStrAsBytes))
                {
                    using (Aes aes = Aes.Create())
                    {
                        using (ICryptoTransform cryptoTransfrm = aes.CreateDecryptor(key, seed))
                        {
                            using (CryptoStream cryptostrm = new CryptoStream(memStrm, cryptoTransfrm, CryptoStreamMode.Read))
                            {
                                cryptostrm.Read(orginalText, 0, orginalText.Length);
                            }
                        }
                    }
                }
                key = seed = null;
                return Encoding.ASCII.GetString(orginalText).Substring(0, len);
            }
            /// <summary>
            /// Function parses the encrypted string for the length of the orginal string
            /// </summary>
            /// <param name="encryptedStr">String to parse from</param>
            /// <returns>Return the length of the original string</returns>
            private int GetLen(ref string encryptedStr)
            {
                char curr = new char();
                int index = 0;
                while (curr != 'X')
                {
                    curr = encryptedStr[index];
                    index++;
                }
                int len = int.Parse(encryptedStr.Substring(0, index - 1), NumberStyles.HexNumber);
                encryptedStr = encryptedStr.Substring(index);
                return len;
            }
            #endregion
        }
        #endregion

        #region Encryption Functions
        /// <summary>
        /// function encrypts the given  str
        /// <para/> function returns an encrypted string
        /// </summary>
        /// <param name="str"> str to encrypt</param>
        /// <param name="keys">place to store encryption keys and Seed</param>
        /// <param name="seed">user defined seed to run encryption on</param>
        /// <returns>returns an encrypted string ro  the name of the file where the file was encrypted</returns>
        public string EncryptStr(string str,ref KeyHolder keys, string seed = null,string key = null)
        {
            if (!CheckFile.checkHasExtention(str))
            {
                int len = str.Length;
                AesEncryption Aes = new AesEncryption();

                if (seed == null && key == null)
                {
                    str = Aes.Encrypt(str);
                }
                else
                {
                    if (seed != null && key == null)
                    {
                        keys = SetSeed(seed, ref Aes);
                        Aes.Seed = keys.Seed;
                        str = Aes.Encrypt(str);
                    }
                    else if (seed == null && key != null)
                    {
                        keys = SetKey(key, ref Aes);
                        Aes.Key = keys.Key;
                        str = Aes.Encrypt(str);
                    }
                    else
                    {
                        keys = SetKey(key, ref Aes);
                        Aes.Key = keys.Key;
                        keys = SetSeed(seed, ref Aes);
                        Aes.Seed = keys.Seed;
                        str = Aes.Encrypt(str);
                    }
                }
                keys = new KeyHolder(Aes.Key, Aes.Seed);
                Aes.FlushKeys();
            }
            return str;
        }
        /// <summary>
        /// function encrypts the given file using Des Encryptor Class
        /// <para>returns a pair that holds key seed and type of encryption used</para>
        /// </summary>
        /// <param name="readLocation"> file to read from</param>
        /// <param name="SaveLocation"> file to write to</param>
        /// <param name="seed"> seed to run encryption algo</param>
        /// <returns>a pair that holds key seed and type of encryption used</returns>
        public KeyHolder Encrypt(string readLocation, string SaveLocation, string seed = null, string key = null)
        {
            KeyHolder tempKeys = null;
            AesEncryption aes = new AesEncryption();
            using (FileStream inputStream = File.Open(readLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream outputStream = File.Open(SaveLocation, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    if (seed == null && key == null)
                    {
                        aes.Encrypt(inputStream, outputStream);
                    }
                    else if (seed != null && key == null)
                    {
                        tempKeys = SetSeed(seed, ref aes);
                        aes.Seed = tempKeys.Seed;
                        aes.Encrypt(inputStream, outputStream);
                    }
                    else if (seed == null && key != null)
                    {
                        tempKeys = SetKey(key, ref aes);
                        aes.Key = tempKeys.Key;
                        aes.Encrypt(inputStream, outputStream);
                    }
                    else
                    {
                        tempKeys = SetKey(key, ref aes);
                        aes.Key = tempKeys.Key;
                        tempKeys = SetSeed(seed, ref aes);
                        aes.Seed = tempKeys.Seed;
                        aes.Encrypt(inputStream, outputStream);
                    }
                }
            }
            tempKeys = new KeyHolder(aes.Key, aes.Seed);
            return tempKeys;
        }
        /// <summary>
        /// function does a try catch on KeyHolder.setSeed 
        /// <para>Returns a KeyHolder obj with a seed intialized</para>
        /// </summary>
        /// <param name="seed">seed to set</param>
        /// <param name="aes"> encryption obj to set encryption if OutOfDomainException is thrown </param>
        /// <returns></returns>
        private KeyHolder SetSeed(string seed,ref AesEncryption aes)
        {
            KeyHolder keys = new KeyHolder();
            try
            {
                keys.setSeed(seed);
            }
            catch (Exception exception)
            {
                if (exception is OutOfDomainException)
                {
                    aes.GenSeed();
                }
                else if (exception is InvalidLengthException)
                {
                    int seedLen = seed.Length;
                    if (seedLen == 1)
                    {
                        seed = "00" + seed;
                        for (int count = 0; count < 4; count++)
                        {
                            seed += seed;
                        }
                        keys.setSeed(seed);
                    }
                    else if (seedLen == 2)
                    {
                        seed = "0" + seed;
                        for (int count = 0; count < 4; count++)
                        {
                            seed += seed;
                        }
                        keys.setSeed(seed);
                    }
                    else
                    {
                        char[] temp = seed.ToCharArray();
                        seed = "";
                        for (int index = 0; index < temp.Length; index++)
                        {
                            seed += "00" + temp[index];
                        }
                        keys.setSeed(seed);
                    }
                }
                else
                {
                    throw;
                }
            }

            return keys;
        }
        /// <summary>
        /// function does a try catch on KeyHolder.setKey 
        /// <para>Returns a KeyHolder obj with a seed intialized</para>
        /// </summary>
        /// <param name="key">key to set</param>
        /// <param name="aes"> encryption obj to set encryption if OutOfDomainException is thrown </param>
        /// <returns></returns>
        private KeyHolder SetKey(string key, ref AesEncryption aes)
        {
            KeyHolder keys = new KeyHolder();
            try
            {
                keys.setKey(key);
            }
            catch (Exception exception)
            {
                if (exception is OutOfDomainException)
                {
                    aes.GenKey();
                }
                else if (exception is InvalidLengthException)
                {
                    int keyLen = key.Length;
                    if (keyLen == 1)
                    {
                        key = "00" + key;
                        for (int count = 0; count < 5; count++)
                        {
                            key += key;
                        }
                        keys.setKey(key);
                    }
                    else if (keyLen == 2)
                    {
                        key = "0" + key;
                        for (int count = 0; count < 5; count++)
                        {
                            key += key;
                        }
                        keys.setKey(key);
                    }
                    else
                    {
                        char[] temp = key.ToCharArray();
                        key = "";
                        for (int index = 0; index < temp.Length; index++)
                        {
                            key += "00" + temp[index];
                        }
                        keys.setKey(key);
                    }
                }
                else
                {
                    throw;
                }
            }

            return keys;
        }
        /// <summary>
        ///  Compresses a given message and encrypts it
        ///  <para>Returns an encrypted string</para>
        /// </summary>
        /// <param name="message">message to compress & encrypt</param>
        /// <param name="seed"> value to use as the seed</param>
        /// <returns>an encrypted stirng</returns>
        public string CompressEncrypt(string message, ref KeyHolder keys, string seed = null,string key = null)
        {
            HuffmanTree hTree = new HuffmanTree();
            return  EncryptStr(hTree.Compress(message), ref keys, seed, key);
        }
        /// <summary>
        /// Compresses a file then encrypts it
        /// </summary>
        /// <param name="inputFile">file to compres and encrypt</param>
        /// <param name="outputFile">file to save to encryption</param>
        /// <param name="seed">value to use as the seed</param>
        public KeyHolder CompressEncrypt(string inputFile,string outputFile, string seed = null, string key = null)
        {
            // run the encryption algo twice / 3 times to create a more psuedo random
            //  encryption so that the look up table  is not found 
            // it would remove the psuedo randomness of the encryption.
            throw new NotImplementedException("read comment on beging of the function");
            HuffmanTree hTree = new HuffmanTree();
            hTree.Compress(inputFile, outputFile);
            return Encrypt(inputFile, outputFile, seed, key);
        }
        #endregion

        #region Decrypion Functions
        /// <summary>
        /// Function decrypts the given string
        /// <para> Returns a decrypted string</para>
        /// </summary>
        /// <param name="encryptedString"> the obj to decrypt</param>
        /// <param name="keys"> Obj to store key and seed</param>
        /// <returns>returns a decrypted string or the name of the decrypted file name</returns>
        public string Decrypt(string encryptedString,KeyHolder keys)
        {
            string decryptedObj = "";
            if (!CheckFile.checkHasExtention(encryptedString))
            {
                AesEncryption aes = new AesEncryption();
                aes.Key = keys.Key;
                aes.Seed = keys.Seed;
                decryptedObj = aes.Decrypt(encryptedString);
            }
            else
            {
                Decrypt(encryptedString, encryptedString, keys);
            }
            return decryptedObj;
        }
        /// <summary>
        /// function  decrypts the given file
        /// </summary>
        /// <param name="readLocation"> file to read</param>
        /// <param name="saveLocation">file to write to</param>
        /// <param name="keys">the holder of decryption key and seed</param>
        public void Decrypt(string readLocation, string saveLocation, KeyHolder keys)
        {
            AesEncryption aes = new AesEncryption();
            aes.Key = keys.Key;
            aes.Seed = keys.Seed;
            using (FileStream inputStream = File.Open(readLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream outputStream = File.Open(saveLocation, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    aes.Decrypt(inputStream, outputStream);
                }
            }
        }
        /// <summary>
        /// Decompresses and Decrypts the given msg
        /// <para>Returns a decrypted & decompressed string</para>
        /// </summary>
        /// <param name="encryptedMsg">msg to decrypt and decompress</param>
        /// <param name="seed"> seed value to use</param>
        /// <returns>returns a decrypted & decompressed string</returns>
        public string DecompressDecrypt(string encryptedMsg, KeyHolder keys)
        {
            throw new NotImplementedException();
        }
        public void DecompressDecrypt(string inputFile, string outputFile, KeyHolder keys)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
