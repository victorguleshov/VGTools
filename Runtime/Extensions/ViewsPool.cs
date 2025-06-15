using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VG.Extensions
{
    public class ViewsPool<T> where T : Component
    {
        private readonly LinkedList<T> activeViews = new();
        private readonly Stack<T> stack = new();

        public ViewsPool(IEnumerable<T> list)
        {
            foreach (var view in list)
            {
                stack.Push(view);
                view.gameObject.SetActive(false);

                Count++;
            }
        }

        public ViewsPool(T prefab, int count = 1)
        {
            prefab.gameObject.SetActive(false);
            stack.Push(prefab);
            Count++;

            for (var i = Count; i < count; i++)
            {
                var view = Instantiate(prefab);
                view.gameObject.SetActive(false);
                stack.Push(view);
                Count++;
            }
        }

        public int Count { get; private set; }
        public int ActiveCount => activeViews.Count;

        private T CreateOrPop()
        {
            if (stack.Count > 0)
            {
                var view = stack.Pop();
                if (activeViews.Count > 0)
                    view.transform.SetSiblingIndex(activeViews.Last.Value.transform.GetSiblingIndex() + 1);

                return view;
            }
            else
            {
                var prefab = activeViews.Last.Value;
                var view = Instantiate(prefab);
                Count++;
                return view;
            }
        }

        private static T Instantiate(T prefab)
        {
            var view = Object.Instantiate(prefab, prefab.transform.parent);
            view.transform.SetSiblingIndex(prefab.transform.GetSiblingIndex() + 1);
            return view;
        }

        public T Get(bool setActive = true)
        {
            var view = CreateOrPop();

            activeViews.AddLast(view);
            if (setActive) view.gameObject.SetActive(true);

            return view;
        }

        public T GetActive(Predicate<T> match)
            => activeViews.FirstOrDefault(activeView => match(activeView));

        public bool TryGetActive(Predicate<T> match, out T result)
        {
            result = GetActive(match);
            return result;
        }

        public void Release(T view)
        {
            if (activeViews.Remove(view))
            {
                stack.Push(view);
                view.gameObject.SetActive(false);
            }
        }
    }
}