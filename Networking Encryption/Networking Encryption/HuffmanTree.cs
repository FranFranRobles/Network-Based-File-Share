using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Networking_Encryption
{
    public class HuffmanTree
    {
        #region BinNode Class
        /// <summary>
        /// Class holds an element a frequency associated to the elment and referencees to other nodes
        /// </summary>
        private class BinNode
        {
            byte Element;
            int freq = 0;
            BinNode Left = null;
            BinNode Right = null;

            private BinNode(byte element, int frequency, ref BinNode left, ref BinNode right)
            {
                Element = element;
                freq = frequency;
                Left = left;
                Right = right;
            }
        }
        #endregion

        private BinNode root = null;
        private byte[] EncodedData = null;

        #region Encode Functions
        /// <summary>
        /// Compresses a given string
        /// <para>Returns a compressed string</para>
        /// </summary>
        /// <param name="inputText"> Text inputed by User </param>
        public string Encode(string inputText)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Compresses a given file
        /// </summary>
        /// <param name="inputFile"> file to compress</param>
        /// <param name="outputFile">place to save compression</param>
        public void Encode(FileStream inputFile, FileStream outputFile)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Decode Functions
        /// <summary>
        /// converts a compressed string back to its original state
        /// </summary>
        /// <param name="encodedStr"> Enconded String</param>
        public string Decode(string encodedStr)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// converts a compressed File back to its original state
        /// </summary>
        /// <param name="inputFile">file to decompress</param>
        /// <param name="outputFile">save location fo decompressed file</param>
        public void Decode(FileStream inputFile, FileStream outputFile)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Make Tree Functions
        /// <summary>
        /// Calculates the Frequency of each differential byte & creates the Huffman Tree
        /// </summary>
        /// <returns></returns>
        private void CalcFreqency()
        {
            throw new NotImplementedException();
            //call make tree function

        }
        /// <summary>
        /// Creates a huffman tree off the freqency list
        /// </summary>
        /// <param name="freqArray"></param>
        private void makeTree(List<BinNode> freqList)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Compression Functions
        /// <summary>
        /// Compresses given data to into a huffman string
        /// </summary>
        /// <param name="data">data to compress</param>
        /// <returns>a huffman string</returns>
        private string Compress(byte[] data)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Compresses  given data into a huffman bin data
        /// </summary>
        /// <param name="data">data to compress</param>
        /// <param name="outputFile">place to save bindata</param>
        private void Compress(byte[] data, FileStream outputFile)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Decompression Functions
        /// <summary>
        /// Decompresses given
        /// <para>Returns a decompressed String</para>
        /// </summary>
        /// <param name="compressedStr">huffman string</param>
        /// <returns>a decoded string</returns>
        private string Decompress(string compressedStr)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Decompresses given data and saves to the outputFile
        /// </summary>
        /// <param name="binData">huffman bin data</param>
        /// <param name="outputFile">place to save decompressed data</param>
        private void Decompress(byte[] binData,FileStream outputFile)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Find/Contains Functions
        /// <summary>
        /// attempts to find the given character within the huffman tree
        /// <para>If found returns frequency of the character, if not found returns -1</para>
        /// </summary>
        /// <param name="character">character to find</param>
        /// <returns>If found returns frequency of the character, if not found returns -1</returns>
        public int Find(string character)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// attempts to find the given byte value within the huffman tree
        /// <para>If found returns frequency of the character, if not found returns -1</para>
        /// </summary>
        /// <param name="value">value to find</param>
        /// <returns>If found returns frequency of the character, if not found returns -1</returns>
        public int Find(byte value)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// attempts to find the binNode associated to the given value
        /// <para>Returns a BinNode if found, if not found return null</para>
        /// </summary>
        /// <param name="value">value to find</param>
        /// <returns>Returns a BinNode if found, if not found return null</returns>
        private BinNode FindNode(byte value)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// function returns true if the given param if found in the huffman tree
        /// </summary>
        /// <param name="character">character to find</param>
        /// <returns>returns true if character if found</returns>
        public bool Contains(string character)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// function returns true if the given param if found in the huffman tree
        /// </summary>
        /// <param name="value">value to find</param>
        /// <returns>returns true if the given value is found</returns>
        public bool Contains(byte value)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Misc functions
        /// <summary>
        /// function clears stored data within the huffman tree
        /// </summary>
        public void Flush()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
