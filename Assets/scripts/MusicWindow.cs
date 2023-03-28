using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts
{
    public class MusicWindow : IEnumerable<AudioClip>
    {
        private readonly int arrayLength;
        private int CurrentIndex;
        public Range WindowRange;

        public MusicWindow(int size, int arrayLength)
        {
            FirstNode = null;
            LastNode = null;
            CurrentNode = null;
            Size = size;
            WindowRange = new Range(0, 0);
            this.arrayLength = arrayLength;
        }

        public Node FirstNode { get; private set; }
        public Node LastNode { get; private set; }
        public int Size { get; set; }
        public Node CurrentNode { get; set; }

        public IEnumerator<AudioClip> GetEnumerator()
        {
            var current = FirstNode;
            while (current is not null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddLast(AudioClip clip)
        {
            var newNode = new Node(clip);
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

        public void AddFirst(AudioClip clip)
        {
            var newNode = new Node(clip);
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


        public async void ShiftRight()
        {
            if (CurrentIndex < Size / 2 && CurrentNode.Next is not null)
            {
                CurrentNode = CurrentNode.Next;
                CurrentIndex++;
            }
            else
            {
                if (CurrentIndex > Size / 2) NormalizeWindow();
                if (CurrentNode.Next is not null)
                {
                    CurrentNode = CurrentNode.Next;
                }
                else
                {
                    await MusicCore.FillWindow();
                    return;
                }

                if (WindowRange.End + 1 == arrayLength) return;
                AddLast(await MusicCore.DownloadNextSong(true));
                WindowRange.Start++;
                FirstNode = FirstNode.Next;
            }
        }

        public async void ShiftLeft()
        {
            if (CurrentIndex > Size / 2 && CurrentNode.Previous is not null)
            {
                CurrentIndex--;
                CurrentNode = CurrentNode.Previous;
            }
            else
            {
                if (CurrentIndex < Size / 2) NormalizeWindow();
                if (CurrentNode.Previous is not null) CurrentNode = CurrentNode.Previous;

                if (WindowRange.Start - 1 < 0) return;
                WindowRange.Start--;
                WindowRange.End--;
                AddFirst(await MusicCore.DownloadNextSong(false));
                LastNode = LastNode.Previous;
            }
        }

        public void SetOutToIndex(int index)
        {
            CurrentIndex = 0;
            CurrentNode = FirstNode;
            while (CurrentIndex != index && CurrentNode.Next is not null)
            {
                CurrentNode = CurrentNode.Next;
                CurrentIndex++;
            }
        }

        private void NormalizeWindow()
        {
            while (CurrentIndex < Size / 2 && WindowRange.Start != 0)
            {
                WindowRange.Start--;
                WindowRange.End--;
                CurrentIndex++;
            }

            while (CurrentIndex > Size / 2 && WindowRange.End != arrayLength)
            {
                WindowRange.End++;
                WindowRange.Start++;
                CurrentIndex--;
            }
        }
    }

    public class Node
    {
        public Node(AudioClip value)
        {
            Next = null;
            Value = value;
            Previous = null;
        }

        public Node Next { get; set; }
        public Node Previous { get; set; }
        public AudioClip Value { get; set; }
    }
}