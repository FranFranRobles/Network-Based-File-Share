using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_Engine.DataStructures
{
    internal abstract class BinaryTree<Key,Elem>
    {
        public abstract void Insert(Key key, Elem elem);
        public abstract Elem Remove(Key k);
        public abstract Elem Find(Key k);
    }
}
