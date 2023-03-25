using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts
{
    internal class WindowEnumerator<T> : IEnumerator<T>
    {
        private Window<T> window;
        private Node<T> node;

        public WindowEnumerator(Window<T> window)
        {
            this.window = window;
            node = null;
        }
        public bool MoveNext()
        {
            node = node == null ? window.FirstNode : node.Next;

            return node is not null;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        object IEnumerator.Current => Current;

        public T Current => node.Value;


        public void Dispose()
        {
            Console.WriteLine("Dispose");
        }
    }
}
