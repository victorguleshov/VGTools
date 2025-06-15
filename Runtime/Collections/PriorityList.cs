using System.Collections;
using System.Collections.Generic;

namespace VG.Collections
{
    public class PriorityList<T> : IEnumerable<T>
    {
        private readonly bool _isMinPriorityList;

        private readonly List<Node> _list = new();

        public PriorityList(bool isMinPriorityList = false)
        {
            _isMinPriorityList = isMinPriorityList;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var node in _list) yield return node.Item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item, int priority)
        {
            var node = new Node { Item = item, Priority = priority };

            if (_isMinPriorityList)
            {
                for (var i = 0; i < _list.Count; i++)
                    if (_list[i].Priority > priority)
                    {
                        _list.Insert(i, node);
                        return;
                    }
            }
            else
            {
                for (var i = 0; i < _list.Count; i++)
                    if (_list[i].Priority < priority)
                    {
                        _list.Insert(i, node);
                        return;
                    }
            }


            _list.Add(node);
        }

        public void Remove(T item)
        {
            for (var i = 0; i < _list.Count; i++)
                if (_list[i].Item.Equals(item))
                {
                    _list.RemoveAt(i);
                    return;
                }
        }

        private class Node
        {
            public T Item { get; set; }
            public int Priority { get; set; }
        }
    }
}