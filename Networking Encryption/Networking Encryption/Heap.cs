using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption
{
    public class Heap<Type>
    {
        #region CTORS
        public Heap()
        {
            throw new NotImplementedException();
        }
        public Heap(Type[] arr)
        {
            throw new NotImplementedException();
        }
        public Heap(Heap<Type> mergeHeap)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Operator overloads
        public static Heap<Type> operator +(Heap<Type> one, Heap<Type> two)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get/Set
        public int Size
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public bool IsEmpty
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        public Type FindMin()
        {
            throw new NotImplementedException();
        }
        public Type Remove()
        {
            throw new NotImplementedException();
        }
        public void Insert(Type element)
        {
            throw new NotImplementedException();
        }
        public Type Replace(Type element)
        {
            throw new NotImplementedException();
        }

    }
}
