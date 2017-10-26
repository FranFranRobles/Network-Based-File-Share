using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption
{
    public class Heap<Type>
    {
        private const int INTIAL_SIZE = 100;
        private int size = 0;
        private Type[] tree = null;
        private Func<Type, Type, bool> compareFunct;

        #region CTORS
        /// <summary>
        /// Default CTOR
        /// <param name="compareFunction">function to compare elements with</param>
        /// </summary>
        public Heap(Func<Type, Type, bool> compareFunction)
        {
            size = 0;
            compareFunct = compareFunction;
            tree = new Type[INTIAL_SIZE];
        }
        /// <summary>
        /// creates a heap off the given input array
        /// </summary>
        /// <param name="compareFunction">function to compare elements with</param>
        /// <param name="arr">input array to create heap off of</param>
        public Heap(Func<Type, Type, bool> compareFunction, Type[] arr)
        {
            tree = new Type[arr.Length];
            compareFunct = compareFunction;
            foreach (Type value in arr)
            {
                Insert(value);
            }
            if (arr.Length != size)
            {
                throw new InvalidLengthException("Failed to intialize correctly");
            }
        }
        /// <summary>
        /// Creates a  deep copy of the given Heap
        /// </summary>
        /// <param name="mergeHeap">heap to copy</param>
        public Heap( Heap<Type> mergeHeap)
        {
            compareFunct = mergeHeap.compareFunct;
            size = mergeHeap.size;
            tree = new Type[mergeHeap.tree.Length];
            int index = 0;
            foreach (Type node in mergeHeap.tree)
            {
                tree[index] = node;
                index++;
            }
            if (size != mergeHeap.size)
            {
                throw new FormatException("Invalid Heap size");
            }
        }
        #endregion

        #region Operator overloads
        public static Heap<Type> operator +(Heap<Type> heapOne, Heap<Type> heapTwo)
        {
            Type[] temp = new Type[heapOne.size + heapTwo.size];
            int index = 0;
            for (int i = 1; i <= heapOne.size; i++)
            {
                temp[index++] = heapOne.tree[i];
            }
            for (int i = 1; i <= heapTwo.size; i++)
            {
                temp[index++] = heapOne.tree[i];
            }
            Heap<Type> newHeap = new Heap<Type>(heapOne.compareFunct,temp);
            if (newHeap.size != heapOne.size + heapTwo.size)
            {
                throw new InvalidLengthException("failed to create to new heap");
            }
            return newHeap;
        }
        #endregion

        #region Get/Set
        /// <summary>
        /// Returns the current amount of elements found within the heap
        /// </summary>
        public int Size
        {
            get
            { return size; }
        }
        /// <summary>
        /// Returns true if the heap is empty else returns false
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                bool isEmpty = false;
                if (size == 0)
                {
                    isEmpty = true;
                }
                return isEmpty;
            }
        }
        #endregion
        /// <summary>
        /// function set / retrives the method of comparing elements within
        /// the heap
        /// </summary>
        public Func<Type,Type,bool> CompareFunction
        {
            get { return compareFunct; }
            set { compareFunct = value; }
        }
        /// <summary>
        /// function returns the lowest value found inside the heap 
        /// </summary>
        /// <returns>the smallest value found inside the heap</returns>
        public Type FindMin()
        {
            return tree[1];
        }
        /// <summary>
        /// function inserts given value into the heap
        /// </summary>
        /// <param name="element"></param>
        public void Insert(Type element)
        {
            if (size + 1 == tree.Length)
            {
                Array.Resize(ref tree, tree.Length * 2);
            }
            size++;
            tree[size] = element;
            int tempLoc = size;
            while (tempLoc > 1)
            {
                tempLoc /= 2;
                if ((2 * tempLoc) + 1 <= size && compareFunct(tree[(2 * tempLoc) + 1], tree[tempLoc]))
                {
                    Swap(ref tree[(2 * tempLoc) + 1], ref tree[tempLoc]);
                }
                if (2 * tempLoc <= size && compareFunct(tree[2 * tempLoc], tree[tempLoc]))
                {
                    Swap(ref tree[2 * tempLoc], ref tree[tempLoc]);
                }
            }
        }
        /// <summary>
        /// function removes the top most element from the heap
        /// <para>Returns the uppermost element of the heap</para>
        /// </summary>
        /// <returns>returns the uppermost element of the heap to the class user</returns>
        public Type Remove()
        {
            Type temp = tree[1];
            tree[1] = tree[size];
            tree[size] = default(Type);
            size--;
            RealignHeap();
            return temp;
        }
        /// <summary>
        /// function removes the uppermost element in the heap & simultainously inserts a new
        /// element into the heap
        /// <para>Returns the uppermost element of the heap</para>
        /// </summary>
        /// <param name="element">param to insert into the heap</param>
        /// <returns>the uppermost element of the heap</returns>
        public Type Replace(Type element)
        {
            Type temp = tree[1];
            tree[1] = element;
            RealignHeap();
            return temp;
        }
        /// <summary>
        /// function swaps the two values of two variables
        /// </summary>
        /// <param name="elemOne">swap one</param>
        /// <param name="elemTwo">swap two</param>
        private void Swap(ref Type elemOne, ref Type elemTwo)
        {
            Type temp = elemOne;
            elemOne = elemTwo;
            elemTwo = temp;
        }
        /// <summary>
        /// Function sorts the heap to mantain the order of the heap valid
        /// </summary>
        private void RealignHeap()
        {
            int CurrLoc = 1;
            while (CurrLoc < size) // mem out of bounds of arr
            {
                if ((2 * CurrLoc) <= size && (2 * CurrLoc + 1 <= size))
                {
                    if (!EqualityComparer<Type>.Default.Equals(tree[2 * CurrLoc + 1], default(Type)) &&
                        !EqualityComparer<Type>.Default.Equals(tree[2 * CurrLoc], default(Type)))
                    {
                        if (compareFunct(tree[2 * CurrLoc], tree[2 * CurrLoc + 1]) &&
                            compareFunct(tree[2 * CurrLoc], tree[CurrLoc]))
                        {
                            Swap(ref tree[2 * CurrLoc], ref tree[CurrLoc]);
                            CurrLoc *= 2;
                        }
                        else if (compareFunct(tree[2 * CurrLoc + 1], tree[CurrLoc]))
                        {
                            Swap(ref tree[2 * CurrLoc + 1], ref tree[CurrLoc]);
                            CurrLoc = 2 * CurrLoc + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else if (2 * CurrLoc <= size && !EqualityComparer<Type>.Default.Equals(tree[2 * CurrLoc],
                    default(Type)) && compareFunct(tree[2 * CurrLoc], tree[CurrLoc]))
                {
                    Swap(ref tree[2 * CurrLoc], ref tree[CurrLoc]);
                    CurrLoc *= 2;
                }
                else
                {
                    break;
                }
                //CurrLoc++;
            }
        }

    }
}