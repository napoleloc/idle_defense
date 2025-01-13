using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using Module.GameUI.InGame.Attribute.Control;
using UnityEngine;

namespace Module.GameUI.InGame.Attribute.GridSheet
{
    public abstract class AttributeControlGridSheet : MonoBehaviour, IAttributeControlGridSheet
    {
        [SerializeField]
        private AttributeControl[] _attributeControls;

        public ReadOnlyMemory<AttributeControl> AttributeControls
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _attributeControls;
        }

        public virtual void Initialize()
        {
            var attributeControls = AttributeControls.Span;

            for (var i = 0; i < attributeControls.Length; i++)
            {
                attributeControls[i].InitializeComponent();
            }
        }

        public virtual void Cleanup()
        {
            var attributeControls = AttributeControls.Span;

            for (var i = 0; i < attributeControls.Length; i++)
            {
                attributeControls[i].Cleanup();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attributeControls.IsNullOrEmpty())
            {
                _attributeControls = GetComponentsInChildren<AttributeControl>();
            }
        }
#endif
    }
}
