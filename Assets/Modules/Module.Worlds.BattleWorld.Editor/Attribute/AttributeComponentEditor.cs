using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Module.Worlds.BattleWorld.Attribute;

namespace Module.Worlds.BattleWorld.Editor.Attribue
{
    using Editor = UnityEditor.Editor;

    public class AttributeComponentEditor : Editor
    {
        public VisualTreeAsset visualTree;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            visualTree.CloneTree(root);
            return root;
        }
    }
}
