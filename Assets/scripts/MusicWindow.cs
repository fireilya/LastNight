using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                CurrentNode= newNode;
                return;
            }

            LastNode.Next = newNode;
            LastNode = newNode;
            WindowRange.End++;
        }

        public void AddFirst(AudioClip clip)
        {
            WindowRange.Start--;
            var newNode = new Node(clip);
            newNode.Next = FirstNode;
            FirstNode ??= newNode;
            if (LastNode is null)
            {
                LastNode = newNode;
                CurrentNode = newNode;
                return;
            }
            FirstNode.Previous = newNode;
            FirstNode = newNode;
        }

        public async Task Next()
        {
            if (CurrentIndex > Size / 2 && WindowRange.End + 1 != arrayLength) await NormalizeWindow();
            if (CurrentIndex < Size / 2 || CurrentNode.Next is not null)
            {
                CurrentNode = CurrentNode.Next;
                CurrentIndex++;
                return;
            }
            if (CurrentNode.Next is not null && WindowRange.End + 1 != arrayLength)
            {
                CurrentNode = CurrentNode.Next;
                await ShiftRight();
                return;
            }
            await MusicCore.FillWindow();
        }

        public async Task Previous()
        {
            if (CurrentIndex < Size / 2 && WindowRange.Start != 0) await NormalizeWindow();
            if (CurrentIndex > Size / 2 || CurrentNode.Previous is not null)
            {
                CurrentNode = CurrentNode.Previous;
                CurrentIndex--;
                return;
            }

            if (CurrentNode.Previous is null || WindowRange.Start == 0) return;
            CurrentNode = CurrentNode.Previous;
            await ShiftLeft();
        }

        private async Task ShiftLeft()
        {
            AddFirst(await MusicCore.DownloadNextSong(false));
            LastNode = LastNode.Previous;
            WindowRange.End--;
        }

        private async Task ShiftRight()
        {
            AddLast(await MusicCore.DownloadNextSong(true));
            WindowRange.Start++;
            FirstNode = FirstNode.Next;
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

        private async Task NormalizeWindow()
        {
            while (CurrentIndex < Size / 2 && WindowRange.Start != 0)
            {
                await ShiftLeft();
                CurrentIndex++;
            }

            while (CurrentIndex > Size / 2 && WindowRange.End+1 != arrayLength)
            {
                await ShiftRight();
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