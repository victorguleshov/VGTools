using System;
using System.Collections;
using System.Collections.Generic;

namespace VG.Collections
{
    public class TinyLinkedList<T> : ICollection<T>
    {
        private Node head;
        private Node tail;

        public TinyLinkedList() { }

        public TinyLinkedList(IEnumerable<T> collection)
        {
            foreach (var item in collection) Add(item);
        }

        public T Head
        {
            get
            {
                if (head == null)
                    throw new InvalidOperationException("The list is empty.");
                return head.Value;
            }
        }

        public T Tail
        {
            get
            {
                if (tail == null)
                    throw new InvalidOperationException("The list is empty.");
                return tail.Value;
            }
        }

        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public void Add(T value)
        {
            var node = new Node(value);

            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                node.Prev = tail;
                tail.Next = node;
                tail = node;
            }

            Count++;
        }

        public bool Remove(T value)
        {
            var current = head;

            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Value, value))
                {
                    if (current.Prev != null)
                        current.Prev.Next = current.Next;
                    else
                        // It's the first node
                        head = current.Next;

                    if (current.Next != null)
                        current.Next.Prev = current.Prev;
                    else
                        // It was the last node
                        tail = current.Prev;

                    Count--;
                    return true;
                }

                current = current.Next;
            }

            return false;
        }

        public void Clear()
        {
            head = null;
            tail = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            var current = head;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Value, item)) return true;

                current = current.Next;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var current = head;
            while (current != null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private Node FindNode(T value)
        {
            var current = head;
            while (current != null)
            {
                if (EqualityComparer<T>.Default.Equals(current.Value, value)) return current;

                current = current.Next;
            }

            return null;
        }

        public void SetHead(T value)
        {
            var node = FindNode(value);
            if (node != null)
            {
                // Remove the node from its current position
                if (node.Prev != null) node.Prev.Next = node.Next;
                if (node.Next != null) node.Next.Prev = node.Prev;
                if (tail == node) tail = node.Prev;

                // Set the node as the new head
                node.Prev = null;
                node.Next = head;
                if (head != null) head.Prev = node;

                head = node;
                tail ??= node;
            }
        }

        public void SetTail(T value)
        {
            var node = FindNode(value);
            if (node != null)
            {
                // Remove the node from its current position
                if (node.Prev != null) node.Prev.Next = node.Next;
                if (node.Next != null) node.Next.Prev = node.Prev;
                if (head == node) head = node.Next;

                // Set the node as the new tail
                node.Next = null;
                node.Prev = tail;
                if (tail != null) tail.Next = node;

                tail = node;
                head ??= node;
            }
        }

        public class Node
        {
            public Node(T value)
            {
                Value = value;
            }

            public T Value { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }
        }
    }
}