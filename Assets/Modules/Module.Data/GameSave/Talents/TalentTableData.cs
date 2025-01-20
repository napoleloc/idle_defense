using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

namespace Module.Data.GameSave.Talents
{
#if UNITY_EDITOR
    using ReadOnly = Sirenix.OdinInspector.ReadOnlyAttribute;
#endif

    [CreateAssetMenu(fileName = nameof(TalentTableData), menuName = "Idle-Defense/Talents/Talent Table Data")]
    public class TalentTableData : ScriptableObject
    {
        private const string UNIQUE_NAME = "talent-file";

        private readonly Dictionary<AttributeType, TalentEntry> _typeToEntryMap = new();
        private readonly Dictionary<AttributeKind, FasterList<AttributeType>> _attributeMapping = new();

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        [ReadOnly]
        private bool _initialized;
        [SerializeField]
        [ReadOnly]
        private TalentEntry[] _entries;

        public ReadOnlyMemory<TalentEntry> Entries
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _entries;
        }

        public void Initialize()
        {
            if(_initialized)
            {
                return;
            }

            if(GameSaveManager.TryGet(UNIQUE_NAME.GetHashCode(), out TalentFile data))
            {
                data.Entries.CopyTo(_entries);

                var map = _attributeMapping;
                var entries = Entries.Span;

                map.Clear();

                for (int i = 0; i < entries.Length; i++)
                {
                    var entry = entries[i]; 

                    if(map.TryGetValue(entry.kind, out var attributes) == false)
                    {
                        attributes = new FasterList<AttributeType>() { entry.type };
                        map[entry.kind] = attributes;
                        continue;
                    }

                    attributes.Add(entry.type);
                }

                _initialized = true;
            }
        }

        public void Deinitialize()
        {
            _entries = Array.Empty<TalentEntry>();
            _attributeMapping.Clear();
            _initialized = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasUnlock(AttributeType type)
            => _typeToEntryMap.ContainsKey(type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(AttributeKind kind, ref NativeArray<AttributeType> attributesToUnlock)
        {
            if (attributesToUnlock.IsCreated == false)
            {
                return false;
            }

            return TryGet(kind, attributesToUnlock);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(AttributeKind kind, Span<AttributeType> attributesToUnlock)
        {
            if(_attributeMapping.TryGetValue(kind, out var attributes))
            {
                attributes.CopyTo(attributesToUnlock);
                return true;
            }

            return false;
        }
    }
}
