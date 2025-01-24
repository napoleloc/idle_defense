using System;
using System.Data;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Vaults;
using Module.GameUI.Talents.Control;
using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

namespace Module.GameUI.Talents.GridSheet
{
    public class TalentControlGridSheet : MonoBehaviour, ITalentControlGridSheet
    {
        private readonly FasterList<TalentControl> _attributeControls = new();

        [Title("Direct Reference", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private Transform _contents;
        [SerializeField]
        private TalentControlPooler _pooler;

        [Title("Soft Reference", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private AttributeKind _attributeKind;

        public ReadOnlyMemory<TalentControl> AttributeControls
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _attributeControls.AsReadOnlyMemory();
        } 

        public virtual void Initialize()
        {
            _attributeKind = AttributeKind.Offensive;
            ReloadGridSheet();
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

        public void OnChangeAttributeKind(AttributeKind kind)
        {
            if(_attributeKind == kind)
            {
                return;
            }

            _attributeKind = kind;
            ReloadGridSheet();
        }

        private void ReloadGridSheet()
        {
            //var count = dataTable.UnlockedCount(_attributeKind);
            //var lenght = _attributeControls.Count - count;

            //if (lenght > 0)
            //{
            //    var span = _attributeControls.AsSpan().Slice(0, lenght);

            //    _pooler.Pool.ReturnComponents(span);
            //    _attributeControls.RemoveAt(0, lenght);
            //}
            //else
            //{
            //    count = count - _attributeControls.Count;
            //    PrepareMany(count);
            //}

            //var amount = _attributeControls.Count;
            //var talentsToUnlock = NativeArray.CreateFast<TalentEntry>(amount, Allocator.Temp);
            //var attributeControls = _attributeControls.AsSpan();

            //if (dataTable.TryGetTalentsToUnlock(_attributeKind, talentsToUnlock))
            //{
            //    for (int i = 0; i < amount; i++)
            //    {
            //        var talent = talentsToUnlock[i];
            //        var talentControl = attributeControls[i];

            //        talentControl.transform.SetParent(_contents);
            //        talentControl.Initialize(talent.Id.SubId);
            //    }
            //}
        }

        private void PrepareMany(int amount)
        {
            if(amount <= 0)
            {
                return;
            }

            var pool = _pooler.Pool;
            var attributeControl = _attributeControls;
            var capacity = attributeControl.Count + amount;

            pool.ReturnComponents(_attributeControls.AsSpan());

            attributeControl.IncreaseCapacityTo(_attributeControls.Count + amount);
            attributeControl.AddReplicateNoInit(amount);

            pool.RentComponents(attributeControl.AsSpan(), true);
        }
    }
}
