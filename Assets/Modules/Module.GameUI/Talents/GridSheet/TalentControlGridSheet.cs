using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Logging;
using Module.Data.GameSave.Talents;
using Module.GameUI.Talents.Control;
using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

namespace Module.GameUI.Talents.GridSheet
{
    public abstract class TalentControlGridSheet : MonoBehaviour, ITalentControlGridSheet
    {
        private readonly FasterList<TalentControl> _attributeControls = new();

        [Title("Soft Reference", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        protected AttributeKind attributeKind;

        [Title("Direct Reference", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private Transform _contents;
        [SerializeField]
        private TalentPool _pool;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        [Sirenix.OdinInspector.ReadOnly]
        private ushort _numTalents;

        public ReadOnlyMemory<TalentControl> AttributeControls
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _attributeControls.AsReadOnlyMemory();
        } 

        public virtual void Initialize(TalentTableData tableData)
        {
            var amount = tableData.Count(attributeKind);

            _attributeControls.AddReplicate(null, amount);

            var attributeControls = _attributeControls.AsSpan();
            _pool.Rent(attributeControls, true);

            //var attributes = NativeArray.CreateFast<AttributeType>(_numTalents, Allocator.Temp);

            for (int i = 0; i < amount; i++)
            {
                attributeControls[i].transform.SetParent(_contents);
            }

            //if (tableData.TryGet(attributeKind, attributes))
            //{
            //    for (int i = 1; i < _numTalents; i++)
            //    {
            //        var type = attributes[i];
            //        attributeControls[i].Initialize(type);
            //    }
            //}
        }

        public virtual void Cleanup()
        {
            var attributeControls = AttributeControls.Span;

            for (var i = 0; i < attributeControls.Length; i++)
            {
                attributeControls[i].Cleanup();
                attributeControls[i].gameObject.SetActive(false);
            }
        }
    }
}
