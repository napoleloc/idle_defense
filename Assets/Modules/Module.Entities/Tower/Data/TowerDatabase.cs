using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Tower.Data
{
    [CreateAssetMenu(fileName = nameof(TowerDatabase), menuName = "Idle-Defense/Entities/Tower/Database")]
    public class TowerDatabase : ScriptableObject
    {
        private readonly Dictionary<int, int> _idToIndexMap = new();

        [SerializeField]
        [AssetSelector(IsUniqueList = true)]
        private TowerConfigAsset[] _entries;

        public ReadOnlyMemory<TowerConfigAsset> Entries
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _entries;
        }

        public void Initialize()
        {
            var map = _idToIndexMap;
            var entries = Entries.Span;
            var entriesLenght = entries.Length;

            map.Clear();
            map.EnsureCapacity(entriesLenght);

            for ( var i = 0; i < entriesLenght; i++)
            {
                var entry = entries[i];

                if(map.TryAdd(i, i) == false)
                {

                }
            }
        }

        public bool TryGetEntry(int id, out TowerConfigAsset entry)
        {
            var result = _idToIndexMap.TryGetValue(id, out var index);
            entry = result ? Entries.Span[index] : default;
            return result;
        }
    }
}
