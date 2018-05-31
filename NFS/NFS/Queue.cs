using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption
{
    public class EmptyQueueException : Exception
    {
        public EmptyQueueException(string msg = null, Exception inner = null)
            : base(msg, inner)
        { }
    }
    public class Queue<Type>
    {
        private const int DEFAULT_CAPACITY = 100;
        private int size;
        private Type[] queue;
        private int front;
        private int back;

        #region Get Functions
        /// <summary>
        /// holds total amount of present items in the queue
        /// </summary>
        public int Size
        {
            get { return size; }
        }
        /// <summary>
        /// is the total number of allocated spaces for inputed items
        /// </summary>
        public int Capacity
        {
            get
            {
                return queue == null ? 0 : queue.Length;
            }
            set
            {
                if (value > Capacity)
                {
                    Resize(value);
                }
            }
        }
        /// <summary>
        /// Holds the amount of space avaiable before needing to increase the capacity of the queue
        /// </summary>
        public int Reserved
        {
            get
            {
                int length = Capacity - size;
                if (length < 0)
                {
                    throw new ArithmeticException("invalid calcuated reserve");
                }
                return length;
            }
        }
        #endregion

        #region CTORS
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Queue()
        {
            size = 0;
            front = back = 0;
            queue = null;
        }
        /// <summary>
        /// Default Constructor with space allocation of inputed amount of 
        /// elements
        /// </summary>
        /// <param name="capacity">size of queue array to intialize</param>
        public Queue(int capacity)
        {
            size = 0;
            front = back = 0;
            queue = new Type[capacity];
        }
        /// <summary>
        /// Creates a queue with the given array
        /// </summary>
        /// <param name="array">input array to intialize queue to</param>
        public Queue(Type[] array)
        {
            size = 0;
            front = back = 0;
            foreach (Type item in array)
            {
                Enque(item);
            }
            if (array.Length != size)
            {
                throw new InvalidLengthException("Queue was not intailized to the correct length");
            }
        }
        /// <summary>
        /// Creates a deep copy of the given queue
        /// </summary>
        /// <param name="cpyQueue">queue to do a deep copy on</param>
        public Queue(Queue<Type> cpyQueue)
        {
            size = cpyQueue.size;
            front = cpyQueue.front;
            back = cpyQueue.back;
            if (size > 0)
            {
                queue = new Type[cpyQueue.queue.Length];
                for (int index = 0; index < queue.Length; index++)
                {
                    queue[index] = cpyQueue.queue[index];
                }
            }
        }
        /// <summary>
        /// Creates a deep copy of the given queue & to addition will allocate space for 
        /// additional nodes for the new queue
        /// </summary>
        /// <param name="capacity">capacity of the new queue</param>
        /// <param name="cpyQueue">queue to do a deep copy from</param>
        public Queue(int capacity, Queue<Type> cpyQueue)
        {
            size = 0;
            front = back = 0;
            queue = new Type[capacity];
            foreach (Type item in cpyQueue.ToArray())
            {
                Enque(item);
            }
            if (size != cpyQueue.size || queue.Length < capacity)
            {
                throw new InvalidLengthException("queue was not intialized correctly");
            }

        }
        /// <summary>
        /// creates a queue off the given array & additionally allocates space for additional nodes
        /// with the given capacity
        /// </summary>
        /// <param name="capacity">capacity of queue to set</param>
        /// <param name="array">array to intialize queue to</param>
        public Queue(int capacity, Type[] array)
        {
            size = 0;
            front = back = 0;
            queue = new Type[capacity];
            foreach (Type item in array)
            {
                Enque(item);
            }
            if (array.Length != size || queue.Length < capacity)
            {
                throw new InvalidLengthException("Queue was not intailized to the correct length");
            }
        }
        #endregion

        /// <summary>
        /// function clears all stored data found within the queue
        /// </summary>
        public void Clear()
        {
            size = 0;
            front = back = 0;
            queue = null;
        }
        /// <summary>
        /// function searches for the given search item
        /// <para>Returns true if the item is found else returns false</para>
        /// </summary>
        /// <param name="searchItem">item to search for</param>
        /// <returns></returns>
        public bool Contains(Type searchItem)
        {
            int count = 0;
            int index = front;
            bool found = false;
            while (found == false && count < size)
            {
                if (index == queue.Length)
                {
                    index = 0;
                }
                if (queue[index].Equals(searchItem))
                {
                    found = true;
                }
                count++;
                index++;
            }

            return found;
        }
        /// <summary>
        /// Function inputs the given item into the back of the queue
        /// </summary>
        /// <param name="inputItem">item to insert into queue</param>
        public void Enque(Type inputItem)
        {
            if (queue == null)// queue is empty
            {
                queue = new Type[DEFAULT_CAPACITY];
            }
            if (size >= queue.Length) // resize queue
            {
                Resize();
            }
            back = back == size ? 0 : back + 1; // end of array
            queue[back] = inputItem; // insert item
            size++; // increase size
        }
        /// <summary>
        /// function removes the item that is in the front of the queue
        /// <para>Returns the removed item to the user</para>
        /// </summary>
        /// <returns> the removed item to the user</returns>
        public Type Deque()
        {
            if (size == 0)
            {
                throw new EmptyQueueException();
            }
            Type tempItem = queue[front];
            queue[front] = default(Type);
            front = front + 1 == queue.Length ? 0 : front + 1;
            size++;
            return tempItem;
        }
        public Type Peek()
        {
            if (size == 0)
            {
                throw new EmptyQueueException();
            }
            return queue[front];
        }

        public Type[] ToArray()
        {
            int index = front;
            Type[] tempArr = new Type[size];
            for (int count = 0; count < size; count++)
            {
                if (index == queue.Length)
                {
                    index = 0;
                }
                tempArr[count] = queue[index];
                index++;
            }
            return tempArr;
        }
        private void Resize(int newSize = -1)
        {
            Type[] tempArr = null;
            if (newSize == -1)
            {
                tempArr = new Type[queue.Length * 2 + 1];
            }
            else if (newSize > queue.Length)
            {
                tempArr = new Type[newSize];
            }
            else
            {
                throw new InvalidLengthException("Size is too small for current queue");
            }
            int index = front;
            for (int count = 0; count < size; count++)
            {
                if (index == queue.Length)
                {
                    index = 0;
                }
                tempArr[count] = queue[index];
                index++;
            }
            queue = tempArr;
            front = 0;
            back = size - 1;
        }

    }
}
