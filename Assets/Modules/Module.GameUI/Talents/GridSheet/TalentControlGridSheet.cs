using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using Google.Apis.Drive.v3.Data;
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
        [Title("Soft Reference", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        protected AttributeKind attributeKind;

        [Title("Direct Reference", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private GameObject _prefabTalentControl;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        [Sirenix.OdinInspector.ReadOnly]
        private int _numTalents;

        private TalentControl[] _attributeControls;

        public ReadOnlyMemory<TalentControl> AttributeControls
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _attributeControls;
        }

        public virtual void Initialize(TalentTableData tableData)
        {
            var attributes = NativeArray.CreateFast<AttributeType>(10, Allocator.Temp);
            if(tableData.TryGet(attributeKind, ref attributes))
            {
                
            }
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
