using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption
{
    /// <summary>
    /// provides manipulation to file Extentions
    /// </summary>
    public class CheckFile
    {
        /// <summary>
        /// function compares the file extentions of two files
        /// <para/>returns true if they hold the same extention 
        /// </summary>
        /// <param name="fileOne"> file one to check </param>
        /// <param name="fileTwo">file two to check</param>
        /// <returns> returns true if the two files contain the same extention </returns>
        public static bool checkExtention(string fileOne, string fileTwo)
        {
            bool areSame = false;
            if (fileOne.Count() >= 4 && fileTwo.Count() >= 4) // 4 is the minimal len for a valid extention
            {
                try
                {
                    if (getExtention(fileOne) == getExtention(fileTwo))
                    {
                        areSame = true;
                    }
                }
                catch (Exception exception)
                {
                    if (exception is FormatException)
                    {
                        return false;
                    }
                    throw; // rethrow any other exception
                }
            }
            return areSame;
        }
        /// <summary>
        /// function finds the extention type of a given file name
        /// <para/>returns a string with the extention type of the file
        /// </summary>
        /// <param name="file"> name of the file</param>
        /// <returns> returns a string associated with the file extention type</returns>
        public static string getExtention(string file)
        {
            int index = file.Count() - 1;
            string extention = "";
            char token = ' ';
            while (token != '.' && index > 0)
            {
                token = file[index];
                extention = extention.Insert(0, token.ToString());
                index--;
            }
            if (!extention.Contains('.'))
            {
                throw new FormatException("not a valid extention");
            }
            return extention;
        }
        /// <summary>
        /// function checks given string has a valid file extention
        /// <para/>returns true if string has an extention
        /// </summary>
        /// <param name="FileName"> string to check</param>
        /// <returns>returns a bool whether or not file has an extention</returns>
        public static bool checkHasExtention(string FileName)
        {
            bool hasExtention = true;
            try
            {
                getExtention(FileName);
            }
            catch (Exception)
            {
                hasExtention = false;
            }
            return hasExtention;
        }
        /// <summary>
        /// Function returns the whole path of a file
        /// </summary>
        /// <param name="path">//name of file to find whole path</param>
        /// <returns></returns>
        public static string GetPath(string path)
        {
            return Directory.GetParent(path).FullName + path;
        }
        #region File Compare Functions
        /// <summary>
        /// function compares two files to check whether they are the same
        /// <para>Returns true if the files are the same</para>
        /// </summary>
        /// <param name="fileOne">First file to check</param>
        /// <param name="compareFile">File to compare with</param>
        /// <returns>returns true if the </returns>
        static public bool CompareFile(string fileOne, string compareFile)
        {
            bool areEqual = false;
            if (CheckFile.checkExtention(fileOne, compareFile))
            {
                var fileOneBytes = readFile(fileOne);
                var fileTwoBytes = readFile(compareFile);
                if (fileOneBytes.Length == fileTwoBytes.Length)
                {
                    int pos = 0;
                    if (fileOneBytes.Length == fileTwoBytes.Length)
                    {
                        bool equal = true;
                        while (pos < fileOneBytes.Length && fileTwoBytes[pos] == fileTwoBytes[pos] && equal == true)
                        {
                            equal = checkBits(fileOneBytes[pos], fileTwoBytes[pos]);
                            pos++;
                        }
                        if (pos == fileOneBytes.Length && pos == fileTwoBytes.Length)
                        {
                            areEqual = true;
                        }
                    }
                }
            }
            return areEqual;
        }
        /// <summary>
        /// function checks whether all of the bits in a byte are the same 
        /// <para>Returns true all the bits are the same</para>
        /// </summary>
        /// <param name="byteOne">first byte to compare</param>
        /// <param name="compareByte"> second byte to compare with</param>
        /// <returns></returns>
        static private bool checkBits(byte byteOne, byte compareByte)
        {
            bool areEqual = false;
            int pos = 0;
            string byteOneBits = "";
            string byteTwoBits = "";
            int temp = 0;
            while (pos < 8)
            {
                temp = (byteOne >> pos) & 0x1;
                byteOneBits += temp == 1 ? temp.ToString() : "0";
                temp = (compareByte >> pos) & 0x1;
                byteTwoBits += temp == 1 ? temp.ToString() : "0";
                pos++;
            }
            if (byteOneBits == byteTwoBits)
            {
                areEqual = true;
            }

            return areEqual;
        }
        /// <summary>
        /// reads in a given file
        /// <para>returns data inside the file as a byte array</para>
        /// </summary>
        /// <param name="path">name of the file</param>
        /// <returns>data of the file in the form of a byte array</returns>
        static private byte[] readFile(string path)
        {
            byte[] fileBytes = null;
            using (FileStream fStrm = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                int len = 0;
                using (BinaryReader binReader = new BinaryReader(fStrm))
                {
                    len = Convert.ToInt32(binReader.BaseStream.Length);
                    fileBytes = binReader.ReadBytes(len);
                }
            }
            return fileBytes;
        }
        #endregion
    }
}
