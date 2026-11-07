#region

using System.Collections.Generic;
using Runtime.UI.Core;
using UnityEngine;

#endregion

namespace Runtime.UI.Common
{
    public class ElementListView<TElement> : MonoBehaviour, IView where TElement : MonoBehaviour, IView
    {
        [SerializeField] private Transform _container;

        private readonly List<TElement> _elements = new();

        public IReadOnlyList<TElement> Elements => _elements;

        public void AddElement(TElement element)
        {
            element.transform.SetParent(_container);
            _elements.Add(element);
        }

        public void RemoveElement(TElement element)
        {
            element.transform.SetParent(null);
            _elements.Remove(element);
        }
    }
}