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
        private bool _editor;
        [SerializeField]
        [ReadOnly]
        private bool _initialized;
        [TableList]
        [SerializeField]
        private TalentEntry[] _entries;

        public ReadOnlyMemory<TalentEntry> Entries
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _entries;
        }

        public int Count(AttributeKind kind)
        {
            var result = _attributeMapping.TryGetValue(kind, out var attributes);
            return result ? attributes.Count : 0;
        }

        public void Initialize()
        {
            if(_initialized)
            {
                return;
            }

            if (_editor == false
                && GameSaveManager.TryGet(UNIQUE_NAME.GetHashCode(), out TalentFile data))
            {
                data.Entries.CopyTo(_entries);
            }

            var map = _attributeMapping;
            var entries = Entries.Span;

            map.Clear();

            for (int i = 0; i < entries.Length; i++)
            {
                var entry = entries[i];

                if (map.TryGetValue(entry.kind, out var attributes) == false)
                {
                    attributes = new FasterList<AttributeType>() { entry.type };
                    map[entry.kind] = attributes;
                    continue;
                }

                attributes.Add(entry.type);
            }

            _initialized = true;
        }

        public void Deinitialize()
        {
            if(_editor == false)
                _entries = Array.Empty<TalentEntry>();

            _attributeMapping.Clear();
            _initialized = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasUnlock(AttributeType type)
            => _typeToEntryMap.ContainsKey(type);

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
