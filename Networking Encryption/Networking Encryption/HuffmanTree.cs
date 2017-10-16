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
        // class constants
        private const int MAX_BYTE_VAL = 256;

        #region BinNode Class
        /// <summary>
        /// Class holds an element a frequency associated to the elment and referencees to other nodes
        /// </summary>
        private class BinNode
        {
            public byte[] Element;
            public int Freq;
            public BinNode Left;
            public BinNode Right;

            public BinNode(byte[] element, int frequency, BinNode left = null, BinNode right = null)
            {
                Element = element;
                Freq = frequency;
                Left = left;
                Right = right;
            }
            public BinNode()
            {
                Element = new byte[0];
                Freq = 0;
                Left = Right = null;
            }
            /// <summary>
            /// Combines the two nodes to create a new one holding the sum of the both parts
            /// </summary>
            /// <param name="leftNode"></param>
            /// <param name="rightNode"></param>
            /// <returns></returns>
            public static BinNode operator +(BinNode leftNode, BinNode rightNode)
            {
                return new BinNode(MergeElements(leftNode.Element, rightNode.Element), 
                    (leftNode.Freq + rightNode.Freq), leftNode, rightNode);
            }
            /// <summary>
            /// Merges the two element arrays into one with the left hand opperand being placed 
            /// first in the array
            /// </summary>
            /// <param name="leftArray">left half of array to merge</param>
            /// <param name="rightArray">right half of array to merge</param>
            private static byte[] MergeElements(byte[] leftArray, byte[] rightArray)
            {
                byte[] newElementArr = new byte[leftArray.Length + rightArray.Length];
                Array.Copy(leftArray, newElementArr, leftArray.Length);
                Array.Copy(rightArray, 0, newElementArr, leftArray.Length, rightArray.Length);
                return newElementArr;
            }
        }
        #endregion

        private BinNode root = null;
        private byte[] EncodedData = null;
        private bool isNull = true;

        #region CTORS
        /// <summary>
        /// Default CTOR
        /// </summary>
        public HuffmanTree()
        {
            root = null;
            EncodedData = null;
            isNull = true;
        }
        #endregion

        #region Get Functions
        /// <summary>
        /// Is true if Tree is completely empty else false
        /// </summary>
        public bool IsNull
        {
            get { return isNull; }
        }
        #endregion

        #region Compress Functions
        /// <summary>
        /// Compresses a given string
        /// <para>Returns a compressed string</para>
        /// </summary>
        /// <param name="inputText"> Text inputed by User </param>
        public string Compress(string inputText)
        {
            EncodedData = Encoding.ASCII.GetBytes(inputText);
            makeTree(CalcFreqency());
            return Encode();
        }
        /// <summary>
        /// Compresses a given file
        /// </summary>
        /// <param name="inputFile"> file to compress</param>
        /// <param name="outputFile">place to save compression</param>
        public void Compress(string inputFile, string outputFile)
        {
            GetFileBytes(inputFile);
            makeTree(CalcFreqency());
            Encode(outputFile);
        }
        #endregion

        #region Decompress Functions
        /// <summary>
        /// converts a compressed string back to its original state
        /// </summary>
        /// <param name="encodedStr"> Enconded String</param>
        public string Decompress(string encodedStr)
        {
            EncodedData = Encoding.ASCII.GetBytes(encodedStr);
            makeTree(FindFreqencies());
            return Decode();
        }
        /// <summary>
        /// converts a compressed File back to its original state
        /// </summary>
        /// <param name="inputFile">file to decompress</param>
        /// <param name="outputFile">save location fo decompressed file</param>
        public void Decompress(string inputFile, string outputFile)
        {
            GetFileBytes(inputFile);
            makeTree(FindFreqencies());
            Decode(outputFile);
        }
        #endregion

        #region Make Tree Functions
        /// <summary>
        /// Calculates the Frequency of each differential byte & creates the Huffman Tree
        /// </summary>
        /// <returns></returns>
        private BinNode[] CalcFreqency()
        {
            if (EncodedData == null)
            {
                throw new NullReferenceException("No data found to Encode");
            }
            int[] freqList = new int[MAX_BYTE_VAL]; // for the domain of a byte[0,255]
            for (int index = 0; index < EncodedData.Length; index++)
            {
                freqList[EncodedData[index]]++;
            }
            BinNode[] nodeList = new BinNode[MAX_BYTE_VAL];
            for (int index = 0; index < MAX_BYTE_VAL; index++)
            {
                byte[] temp = new byte[1];
                temp[0] = (byte)index;
                nodeList[index] = new BinNode(temp, freqList[index]);
            }
            return nodeList;
        }
        private BinNode[] FindFreqencies()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Creates a huffman tree off the freqency list
        /// </summary>
        /// <param name="freqList">list to make </param>
        private void makeTree(BinNode[] freqList)
        {
            if (freqList.Length <= 0)
            {
                throw new ArgumentException();
            }
            Heap<BinNode> heap = new Heap<BinNode>(CompareFunction, freqList);
            while (!heap.IsEmpty)
            {
                heap.Insert(heap.Remove() + heap.Remove());
            }
            root = heap.Remove();
        }

        /// <summary>
        /// function combines two element list to one
        /// </summary>
        /// <param name="element1">first half</param>
        /// <param name="element2">second half</param>
        /// <returns></returns>
        private byte[] CreateElement(byte[] element1, byte[] element2)
        {
            byte[] temp = new byte[element1.Length + element2.Length];
            int index = 0;
            foreach (byte num in element1)
            {
                temp[index] = num;
                index++;
            }
            foreach (byte num in element2)
            {
                temp[index] = num;
                index++;
            }
            return temp;
        }
        #endregion

        #region Encode Functions
        /// <summary>
        /// Compresses given data to into a huffman string
        /// </summary>
        /// <returns>a huffman string</returns>
        private string Encode()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Compresses  given data into a huffman bin data
        /// </summary>
        /// <param name="data">data to compress</param>
        /// <param name="outputFile">place to save bindata</param>
        private void Encode(string outputFile)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Decode Functions
        /// <summary>
        /// Decompresses given
        /// <para>Returns a decompressed String</para>
        /// </summary>
        /// <param name="compressedStr">huffman string</param>
        /// <returns>a decoded string</returns>
        private string Decode()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Decompresses given data and saves to the outputFile
        /// </summary>
        /// <param name="binData">huffman bin data</param>
        /// <param name="outputFile">place to save decompressed data</param>
        private void Decode(string outputFile)
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
        /// function  compares two nodes  left < right
        /// <para> if left param is less than right returns true</para>
        /// </summary>
        /// <param name="left">left node of the eniquality</param>
        /// <param name="right">right node of the eniquality</param>
        /// <returns></returns>
        private bool CompareFunction(BinNode left,BinNode right)
        {
            return left.Freq < right.Freq ? true : false;
        }
        /// <summary>
        /// function clears stored data within the huffman tree
        /// </summary>
        public void Flush()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// function gets all of the bytes found inside a file &  sets the encodedData[]
        /// attribute of the class
        /// </summary>
        /// <param name="inputFile">file to get all bytes from</param>
        private void GetFileBytes(string inputFile)
        {
            using (FileStream inputStrm = new FileStream(inputFile,FileMode.Open,FileAccess.Read))
            {
                using (BinaryReader binReader = new BinaryReader(inputStrm))
                {
                    long len = new FileInfo(inputFile).Length;
                    EncodedData = binReader.ReadBytes((int)len);
                }
            }
            throw new NotImplementedException();
        }
        #endregion
    }
}
