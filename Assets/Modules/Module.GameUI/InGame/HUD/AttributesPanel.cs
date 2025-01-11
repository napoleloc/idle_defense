using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using TMPro;
using UnityEngine;

namespace Module.GameUI.InGame
{
    public abstract class AttributesPanel : MonoBehaviour, IAttributesPanel
    {
        [SerializeField]
        protected AttributeControl[] attributeControls;

        public ReadOnlyMemory<AttributeControl> AttributeControls
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => attributeControls;
        }

        public virtual void Initialize()
        {
            var controls = AttributeControls.Span;
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].InitializeComponent();
            }
        }
        public virtual void Cleanup()
        {
            var controls = AttributeControls.Span;
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].Cleanup();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (attributeControls.IsNullOrEmpty())
            {
                attributeControls = GetComponentsInChildren<AttributeControl>();
            }
        }
#endif
    }
}
