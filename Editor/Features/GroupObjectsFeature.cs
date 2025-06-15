using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PlatformerDemo.Editor
{
    public class GroupObjectsFeature
    {
        private static Transform[] _lastSelectedTransforms;
        private static int _selectedItemsLeft;
        private static Dictionary<Transform, int> _hierarchyIndexes;


        [MenuItem("GameObject/Group Selected  %g", false, 0)]
        public static void GroupSelected(MenuCommand menuCommand)
        {
            if (_lastSelectedTransforms != null)
                if (_lastSelectedTransforms.SequenceEqual(Selection.transforms))
                {
                    _selectedItemsLeft--;
                    if (_selectedItemsLeft <= 0) _lastSelectedTransforms = null;

                    return;
                }

            if (!Selection.activeTransform) return;

            _selectedItemsLeft = Selection.transforms.Length - 1;

            var go = new GameObject("Group");

            Undo.RegisterCreatedObjectUndo(go, "Group Selected");

            var groupParent = Selection.activeTransform.parent;
            var parentFound = false;
            _hierarchyIndexes = new Dictionary<Transform, int>();
            var groupSiblingIndex = 999999999;

            foreach (var transform in Selection.transforms)
            {
                if (!parentFound)
                    if (!Array.Exists(Selection.transforms, element => element == transform.parent))
                    {
                        groupParent = transform.parent;
                        parentFound = true;
                    }

                _hierarchyIndexes[transform] = transform.GetSiblingIndex();

                groupSiblingIndex = Math.Min(groupSiblingIndex, transform.GetSiblingIndex());
            }

            go.transform.SetParent(groupParent, false);
            go.transform.SetSiblingIndex(groupSiblingIndex);

            IEnumerable<Transform> sortedByIndex = Selection.transforms.OrderByDescending(t => _hierarchyIndexes[t]);

            go.transform.position = sortedByIndex.Last().position;


            foreach (var transform in Selection.transforms)
                Undo.SetTransformParent(transform, go.transform, "Group Selected");

            foreach (var item in sortedByIndex) item.SetAsFirstSibling();

            Selection.activeGameObject = go;

            _hierarchyIndexes = null;

            if (menuCommand.context) _lastSelectedTransforms = Selection.transforms;
        }
    }
}