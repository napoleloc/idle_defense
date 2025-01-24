using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using Module.Data.GameSave;
using Module.Data.GameSave.Talents;
using Module.Worlds.BattleWorld.Attribute;
using UnityEngine;

namespace Module.Data.Runtime.Talents
{
    [CreateAssetMenu(fileName = nameof(RuntimeTalentDataTableAsset), menuName = "Idle-Defense/Rumtime-Data/Talent Table Data")]
    public class RuntimeTalentDataTableAsset : RuntimeDataTableAsset
    {
        private const string TALENT_UNIQUE_NAME = "talent-file";

        private readonly Dictionary<AttributeKind, FasterList<TalentEntry>> _unlockedTalents = new();
        private readonly Dictionary<TalentIdData, int> _idToIndexMap = new();

        private FasterList<TalentEntry> _entries = new();

        public ReadOnlyMemory<TalentEntry> Entries
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _entries.AsReadOnlyMemory();
        }

        protected internal override void Initialize()
        {
            if (GameSaveManager.TryGet<TalentFile>(TALENT_UNIQUE_NAME.GetHashCode(), out var data))
            {
                var entries = _entries;
                var count = data.UnlockedCount;

                entries.AddReplicateNoInit(count);
                data.GetEntries(entries.AsSpan());

                for (var i = 0; i < count; i++)
                {
                    TryAddEntry(Entries.Span[i]);
                }
            }
        }

        protected internal override void Deinitialize()
        {
            if (GameSaveManager.TryGet<TalentFile>(TALENT_UNIQUE_NAME.GetHashCode(), out var data))
            {
                data.AddEntries(Entries.Span);
            }
        }

        public bool TryAddEntry(TalentEntry entry)
        {
            var unlockedTalents = _unlockedTalents;
            var map = _idToIndexMap;
            var entries = _entries;
            var id = GetId(entry);

            if (map.ContainsKey(id) == false)
            {
                var index = map.Count;
                var capacity = index + 1;

                map.EnsureCapacity(capacity);
                map.TryAdd(id, index);
                entries.Add(entry);

                if (unlockedTalents.TryGetValue(id.Kind, out var talentsToUnlock) == false)
                {
                    talentsToUnlock = new FasterList<TalentEntry>(entry);
                    unlockedTalents[id.Kind] = talentsToUnlock;
                }
                else
                {
                    talentsToUnlock.Add(entry);
                }

                return true;
            }

            return false;
        }

        public int UnlockedCount(AttributeKind kind)
        {
            var result = _unlockedTalents.TryGetValue(kind, out var talentsToUnlock);
            return result ? talentsToUnlock.Count : 0;
        }

        public bool TryGetTalentsToUnlock(AttributeKind kind, Span<TalentEntry> entriesToUnlock)
        {
            var result = _unlockedTalents.TryGetValue(kind, out var entries);
            if (result)
            {
                entries.CopyTo(entriesToUnlock);
            }

            return result;
        }

        public bool TryGetEntry(TalentIdData id, out TalentEntry entry)
        {
            var result = _idToIndexMap.TryGetValue(id, out var index);
            entry = result ? Entries.Span[index] : default(TalentEntry);
            return result;
        }

        public bool TryRemoveEntry(TalentEntry entry)
        {
            var id = GetId(entry);

            if (_idToIndexMap.ContainsKey(id))
            {
                _idToIndexMap.Remove(id);
                _entries.Remove(entry);

                if (_unlockedTalents.TryGetValue(id.Kind, out var talentsToUnlock))
                {
                    talentsToUnlock.Remove(entry);
                }

                return true;
            }

            return false;
        }

        private TalentIdData GetId(in TalentEntry entry)
            => entry.Id;
    }
}
