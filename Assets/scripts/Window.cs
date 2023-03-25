using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.scripts
{
    public class Window <T> : IEnumerable<T>
    {
        public Window(int size, int arrayLength)
        {
            FirstNode = null;
            LastNode = null;
            CurrentNode = null;
            Size = size;
            WindowRange = new Range(0, 0);
            this.arrayLength= arrayLength;
        }
        public Node<T> FirstNode { get; private set; }
        public Node<T> LastNode { get; private set; }
        public Range WindowRange;
        public int Size { get; set; }
        public Node<T> CurrentNode { get; set; }
        private int CurrentIndex;
        private int arrayLength;

        public void AddLast(T clip)
        {
            var newNode = new Node<T>(clip);
            newNode.Previous = LastNode;
            FirstNode ??= newNode;
            if (LastNode is null)
            {
                LastNode = newNode;
                return;
            }

            LastNode.Next = newNode;
            LastNode = newNode;
            WindowRange.End++;
        }

        public void AddFirst(T clip)
        {
            var newNode = new Node<T>(clip);
            newNode.Next = FirstNode;
            FirstNode.Previous = newNode;
            FirstNode = newNode;
        }

        public void Clear()
        {
            FirstNode = null;
            LastNode = null;
            WindowRange.Start = 0;
            WindowRange.End = 0;
        }


        public void ShiftRight(T clip)
        {
            if (CurrentIndex<4)
            {
                CurrentNode = CurrentNode.Next;
                CurrentIndex++;
            }
            else
            {
                if (CurrentIndex>4)
                {
                    NormalizeWindow();
                }
                if (CurrentNode.Next is not null)
                {
                    CurrentNode = CurrentNode.Next;
                }
                else
                {
                    MusicCore.FillWindow();
                    return;
                }

                if (WindowRange.End + 1 == arrayLength) return;
                AddLast(clip);
                WindowRange.Start++;
                FirstNode = FirstNode.Next;
            }
        }

        public void ShiftLeft(T clip)
        {
            if (CurrentIndex > 4)
            {
                CurrentIndex--;
                CurrentNode= CurrentNode.Previous;
            }
            else
            {
                if (CurrentIndex<4)
                {
                    NormalizeWindow();
                }
                if (CurrentNode.Previous is not null)
                {
                    CurrentNode = CurrentNode.Previous;
                }

                if (WindowRange.Start - 1 < 0) return;
                WindowRange.Start--;
                WindowRange.End--;
                AddFirst(clip);
                LastNode = LastNode.Previous;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new WindowEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void SetOutToIndex(int index)
        {
            CurrentIndex = 0;
            CurrentNode = FirstNode;
            while (CurrentIndex!=index)
            {
                CurrentNode = CurrentNode.Next;
                CurrentIndex++;
            }
        }

        private void NormalizeWindow()
        {
            while (CurrentIndex < 4 && WindowRange.Start != 0)
            {
                WindowRange.Start--;
                WindowRange.End--;
                CurrentIndex++;
            }

            while (CurrentIndex>4 && WindowRange.End!=arrayLength)
            {
                WindowRange.End++;
                WindowRange.Start++;
                CurrentIndex--;
            }
        }
    }

    public class Node<T>
    {
        public Node(T value)
        {
            Next = null;
            Value = value;
            Previous = null;
        }
        public Node(T value, Node<T> next, Node<T> previous)
        {
            Next = next;
            Value = value;
            Previous = previous;
        }

        public Node<T> Next { get; set; }
        public Node<T> Previous { get; set; }
        public T Value { get; set; }
    }
}