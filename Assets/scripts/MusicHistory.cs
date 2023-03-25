using UnityEngine;

namespace Assets.scripts
{
    public class MusicHistory
    {
        public MusicHistory()
        {
            FirstNode = null;
            LastNode = null;
        }

        public int Count { get; private set; }
        public Node FirstNode { get; private set; }
        public Node LastNode { get; private set; }

        public Node CurrentNode { get; set; }

        public void Add(AudioClip clip)
        {
            var newNode = new Node(clip);
            Count++;
            newNode.Previous = LastNode;
            FirstNode ??= newNode;
            if (LastNode is null)
            {
                LastNode = newNode;
                return;
            }

            LastNode.Next = newNode;
            LastNode = newNode;
            CurrentNode = newNode;
        }

        public void Clear()
        {
            FirstNode = null;
            LastNode = null;
            Count = 0;
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