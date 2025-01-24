using System.Collections.Generic;
using EncosyTower.Modules.Collections;
using Module.Worlds.BattleWorld.Attribute;
using Unity.Collections;
using UnityEngine;

namespace Module.Data.Runtime.Talents
{
    [CreateAssetMenu(fileName = nameof(TalentDataTableAsset), menuName = "Idle-Defense/Talents/Talent Table Data")]
    public class TalentDataTableAsset : DataTable<TalentIdData, TalentEntry>
    {
        private readonly Dictionary<AttributeKind, FasterList<TalentEntry>> _unlockedTalents;

        protected override TalentIdData GetId(in TalentEntry data)
            => data.Id;

        protected internal override void Initialize()
        {
            base.Initialize();

            var map = _unlockedTalents;
            var entries = Entries.Span;

            map.Clear();
            map.EnsureCapacity(entries.Length);

            for (var i = 0; i < entries.Length; i++)
            {
                var id = GetId(entries[i]);
                var kind = id.Kind;

                if (map.TryGetValue(kind, out var talents) == false)
                {
                    talents = new() { entries[i] };
                    map.Add(kind, talents);
                    continue;
                }

                talents.Add(entries[i]);
            }
        }

        public int UnlockedCount(AttributeKind kind)
        {
            var result = _unlockedTalents.TryGetValue(kind, out var talentsToUnlock);
            return result ? talentsToUnlock.Count : 0;
        }

        public bool TryGetTalentsToUnlock(AttributeKind kind, ref NativeArray<TalentEntry> entriesToUnlock)
        {
            if (entriesToUnlock.IsCreated)
            {
                return false;
            }

            var result = _unlockedTalents.TryGetValue(kind, out var entries);
            if (result)
            {
                entries.CopyTo(entriesToUnlock);
            }

            return result;
        }
    }
}
