using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Security.Cryptography;
/*
 * Build: 0.5.4
 * Date: 7/13/17
 * Code Metrics:
 * Network Encryption: 777  86  1   17  213
 * Unit Tests: 61   28   1   5   268
 * 
 * 53.6627552333
 */

namespace Networking_Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            uint[] test = new uint[1000];
            var a = new Random();
            for (int i = 0; i < test.Length; i++)
            {
                test[i] = (uint)a.Next();
            }
            //Heap<uint> d = new Heap<uint>(CompareFunction, test);
        }
        public bool CompareFunction(uint left, uint right)
        {
            return left < right ? true : false;
        }
    }
}
