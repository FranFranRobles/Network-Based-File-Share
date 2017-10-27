using System;
using System.Collections.Generic;

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
        /// <summary>
        /// function takes two heaps and concatenates them into a third heap 
        /// <para>Note: function will use the compare function of the left Param</para>
        /// </summary>
        /// <param name="heapOne">left param</param>
        /// <param name="heapTwo">right param</param>
        /// <returns>returns a new heap with the value the values of the concatenation of the two s
        ///         source heaps</returns>
        public static Heap<Type> operator +(Heap<Type> heapOne, Heap<Type> heapTwo)
        {
            // list contains the values of heapOne & heapTwo
           Type[] list = new Type[heapOne.size + heapTwo.size];
            int listIndex = 0;
            for (int heapOneIndex = 1; heapOneIndex <= heapOne.size; heapOneIndex++)
            {
                list[listIndex++] = heapOne.tree[heapOneIndex];
            }
            for (int heapTwoIndex = 1; heapTwoIndex <= heapTwo.size; heapTwoIndex++)
            {
                list[listIndex++] = heapOne.tree[heapTwoIndex];
            }
            // new heap  = heapOne + heapTwo
            Heap<Type> newHeap = new Heap<Type>(heapOne.compareFunct,list);
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
        }
        /// <summary>
        /// function returns the either the max or the min of the heap specified by
        /// function inputed by the user
        /// </summary>
        /// <returns>the smallest value found inside the heap</returns>
        public Type FindTop()
        {
            return tree[1];
        }
        /// <summary>
        /// function inserts given value into the heap
        /// </summary>
        /// <param name="element">value to insert into heap</param>
        public void Insert(Type element)
        {
            if (size + 1 == tree.Length)
            {
                Array.Resize(ref tree, tree.Length * 2);
            }
            size++;
            tree[size] = element; // insert new element
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
            if (IsEmpty == true)
            {
                throw new InvalidOperationException(" no data to replace");
            }
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
            bool swaped = true;
            while (CurrLoc < size && swaped == true) // mem out of bounds of arr
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
                            swaped = false;
                        }
                    }
                    else
                    {
                        swaped = false;
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
                    swaped = false;
                }
            }
        }

    }
}