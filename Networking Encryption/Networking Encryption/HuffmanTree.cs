using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Networking_Encryption
{
    public class HuffmanTree
    {
        
        private const int MAX_BYTE_VAL = 256;

        #region BinNode Class
        /// <summary>
        /// Class holds an element a frequency associated to the elment and referencees to other nodes
        /// </summary>
        private class BinNode
        {
            public byte[] Element;
            public uint Freq;
            public BinNode Left;
            public BinNode Right;

            public BinNode(byte[] element, uint frequency, BinNode left = null, BinNode right = null)
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
        private string[] hmanCode = null;
        private bool isNull = true;
        private ulong comprssdLen = 0;
        private uint[] freqList = null;

        #region CTORS
        /// <summary>
        /// Default CTOR
        /// </summary>
        public HuffmanTree()
        {
            root = null;
            EncodedData = null;
            freqList = null;
            isNull = true;
            comprssdLen = 0;
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
            MakeTree(CalcFreqency());
            GetEncodes();
            byte[] compressedData = Encode();
            isNull = false;
            return GetLen().ToString() + GetFrq().ToString() + compressedData.ToString();
        }
        /// <summary>
        /// Compresses a given file
        /// </summary>
        /// <param name="inputFile"> file to compress</param>
        /// <param name="outputFile">place to save compression</param>
        public void Compress(string inputFile, string outputFile)
        {
            ReadFile(inputFile);
            MakeTree(CalcFreqency());
            GetEncodes();
            using (FileStream output = new FileStream(outputFile, FileMode.Truncate, FileAccess.Write))
            {
                byte[] temp = Encode();
                byte[] len = GetLen();
                byte[] freqByte = GetFrq();
                output.Write(len, 0, len.Length);
                //output.Write(freqByte, 0, freqByte.Length);
                //output.Write(temp, 0, temp.Length);
            }
            isNull = false;
        }

        private byte[] GetFrq()
        {
            List<byte> holder = new List<byte>();
            byte[] temp = null;
            foreach (var item in freqList)
            {
                temp = BitConverter.GetBytes(item);
                holder.AddRange(temp);
            }
            return holder.ToArray();
        }
        #endregion

        #region Decompress Functions
        /// <summary>
        /// converts a compressed string back to its original state
        /// </summary>
        /// <param name="encodedStr"> Enconded String</param>
        public string Decompress(string encodedStr)
        {
            byte[] encodedData = GetLen(Encoding.ASCII.GetBytes(encodedStr));
            MakeTree(encodedData);
            return Decode();
        }
        private void MakeTree(byte[] compressedData)
        {
            List<byte> lst = new List<byte>();
            freqList = new uint[MAX_BYTE_VAL];
            int comCount = 0;
            uint temp = 0;
            for (int index = 0; index < freqList.Length; index++)
            {
                lst.Add(compressedData[comCount]);
                lst.Add(compressedData[comCount + 1]);
                lst.Add(compressedData[comCount + 2]);
                lst.Add(compressedData[comCount + 3]);
                temp = BitConverter.ToUInt32(lst.ToArray(),lst.Count);
                lst.RemoveRange(0,lst.Count);
            }
            throw new NotImplementedException();
        }
        /// <summary>
        /// converts a compressed File back to its original state
        /// </summary>
        /// <param name="inputFile">file to decompress</param>
        /// <param name="outputFile">save location fo decompressed file</param>
        public void Decompress(string inputFile, string outputFile)
        {
            ReadFile(inputFile);
            MakeTree(FindFreqencies());
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
            freqList = new uint[MAX_BYTE_VAL]; // for the domain of a byte[0,255]
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
        private void MakeTree(BinNode[] freqList)
        {
            if (freqList.Length <= 0)
            {
                throw new ArgumentException();
            }
            Heap<BinNode> heap = new Heap<BinNode>(CompareFunction, freqList);
            while (heap.Size  > 1)
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
        private byte[] Encode()
        {

            List<byte> encodedData = new List<byte>();
            byte temp = 0;
            int count = 0;
            for (int index = 0; index < EncodedData.Length; index++)
            {
                string code = hmanCode[EncodedData[index]];
                for (int i = 0; i < code.Length; i++)
                {
                    if (code[i] == '1')
                    {
                        temp |= (byte)(1 << (count % 8));
                    }
                    if ((count + 1) % 8 == 0 && index < EncodedData.Length - 1)
                    {
                        encodedData.Add(temp);
                        temp = 0;
                    }
                    count++;
                }
            }
            encodedData.Add(temp);
            comprssdLen = (ulong)count;
            return encodedData.ToArray();
        }
        /// <summary>
        /// function retrieves the encodes for all possible values
        /// <para>returns </para>
        /// </summary>
        /// <returns></returns>
        private void GetEncodes()
        {
            var tasks = new List<Task<string>>();
            hmanCode = new string[MAX_BYTE_VAL];
            for (int value = 0; value < hmanCode.Length; value++)
            {
                tasks.Add(Task<string>.Factory.StartNew(() => GetCode((byte)value)));
            }
            Task.WaitAll(tasks.ToArray());
            for (int index = 0; index < hmanCode.Length; index++)
            {
                hmanCode[index] = tasks[index].Result;
            }
        }
        /// <summary>
        /// function retrieves the huffman code for a given prefix
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetCode(byte prefix)
        {
            string code = "";
            var temp = root;
            while (!(temp.Element.Length == 1) && temp.Element.Contains(prefix))
            {
                bool found = temp.Left.Element.Contains(prefix);
                temp = found == true ? temp.Left : temp.Right;
                code += found ? "0" : "1";

            }
            return code;
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
        public long Find(string character)
        {
            if (character.Length != 1)
            {
                throw new InvalidLengthException("string length need to be 1 for you to do a search");
            }
            BinNode temp = FindNode(Encoding.ASCII.GetBytes(character)[0]);
            return temp != null ? (long)temp.Freq : -1;
        }
        /// <summary>
        /// attempts to find the given byte value within the huffman tree
        /// <para>If found returns frequency of the character, if not found returns -1</para>
        /// </summary>
        /// <param name="value">value to find</param>
        /// <returns>If found returns frequency of the character, if not found returns -1</returns>
        public long Find(byte value)
        {
            BinNode temp = FindNode(value);
            return temp != null ? (long)temp.Freq : -1;
        }
        /// <summary>
        /// attempts to find the binNode associated to the given value
        /// <para>Returns a BinNode if found, if not found return null</para>
        /// </summary>
        /// <param name="value">value to find</param>
        /// <returns>Returns a BinNode if found, if not found return null</returns>
        private BinNode FindNode(byte value)
        {
            BinNode curr = root;
            while (curr.Element.Length > 1 || curr.Element[0] != value)
            {
                curr = curr.Left.Element.Contains(value) ? curr.Left : curr.Right;
            }
            if (!curr.Element.Contains(value))
            {
                curr = null;
            }
            return curr;
        }
        /// <summary>
        /// function returns true if the given param if found in the huffman tree
        /// with a frequency greater than 0
        /// </summary>
        /// <param name="character">character to find</param>
        /// <returns>returns true if character if found</returns>
        public bool Contains(string character)
        {
            if (character.Length != 1)
            {
                throw new InvalidLengthException("string length need to be 1 for you to do a search");
            }
            bool found = false;
            BinNode temp = FindNode(Encoding.ASCII.GetBytes(character)[0]);
            if (temp != null)
            {
                found = temp.Freq > 0 ? true : false;
            }
            return found;
        }
        /// <summary>
        /// function returns true if the given param if found in the huffman tree
        /// with a frequency greater than 0
        /// </summary>
        /// <param name="value">value to find</param>
        /// <returns>returns true if the given value is found</returns>
        public bool Contains(byte value)
        {
            bool found = false;
            BinNode temp = FindNode(value);
            if (temp != null)
            {
                found = temp.Freq > 0 ? true : false;
            }
            return found;
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
            freqList = null;
            comprssdLen = 0;
            EncodedData = null;
            hmanCode = null;
            isNull = true;
            root = null;
        }
        /// <summary>
        /// returns a binary hex reprsentation of the length of the compressed data
        /// </summary>
        /// <returns></returns>
        private byte[] GetLen()
        {
            return BitConverter.GetBytes(comprssdLen);
        }
        /// <summary>
        /// function reads the the length of the compressed data from the compressedData array
        /// returns a sub array of the compressed data
        /// </summary>
        /// <param name="compressedData"></param>
        /// <returns></returns>
        private byte[] GetLen(byte[] compressedData)
        {
            byte[] lenBytes = new byte[8];
            byte[] destArr = new byte[compressedData.Length - 8]; 
            for (int index = 0; index < lenBytes.Length; index++)
            {
                lenBytes[index] = compressedData[index];
            }
            comprssdLen = BitConverter.ToUInt64(lenBytes, 0);
            Array.Copy(compressedData, destArr,compressedData.Length - 8);
            return destArr;
        }
        /// <summary>
        /// function gets all of the bytes found inside a file &  creates the encodedData[]
        /// attribute of the class
        /// </summary>
        /// <param name="inputFile">file to get all bytes from</param>
        private void ReadFile(string inputFile)
        {
            using (FileStream inputStrm = new FileStream(inputFile,FileMode.Open,FileAccess.Read))
            {
                using (BinaryReader binReader = new BinaryReader(inputStrm))
                {
                    long len = new FileInfo(inputFile).Length;
                    EncodedData = binReader.ReadBytes((int)len);
                }
            }
        }
        #endregion
    }
}
