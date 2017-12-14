using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption
{
    public class Queue<Type>
    {
        private uint size;
        private Type[] queue;
        private uint front;
        private uint back;

        #region Get Functions
        public uint Size
        {
            get { return size; }
        }
        public int Capacity
        {
            get { return queue.Length; }
        }
        #endregion

        #region CTORS
        public Queue()
        {
            size = 0;
            front = back = 0;
            queue = null;
        }
        public Queue(uint capacity)
        {
            size = 0;
            front = back = 0;
            queue = new Type[capacity];
        }
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
        public Queue(Queue<Type> cpyQueue)
        {
            size = cpyQueue.size;
            front = cpyQueue.front;
            back = cpyQueue.back;
            queue = new Type[cpyQueue.queue.Length];
            for (int index = 0; index < queue.Length; index++)
            {
                queue[index] = cpyQueue.queue[index];
            }
        }
        public Queue(uint capacity, Queue<Type> queue)
        {
            // need to fix copy constructor test
            throw new NotImplementedException();
        }
        public Queue(uint capacity, Type[] array)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Clear()
        {
            throw new NotImplementedException();
        }
        public bool Contains(Type searchVal)
        {
            throw new NotImplementedException();
        }
        public void Enque(Type val)
        {
            throw new NotImplementedException();
        }
        public Type Deque()
        {
            throw new NotImplementedException();
        }
        public Type Peek()
        {
            throw new NotImplementedException();
        }

        public Type[] ToArray()
        {
            throw new NotImplementedException();
        }

    }
}
