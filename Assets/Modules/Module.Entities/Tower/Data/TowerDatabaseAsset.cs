using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Module.Entities.Tower.Data
{
    using ConfigAssetRef = LazyLoadReference<TowerConfigAsset>;

    [CreateAssetMenu(fileName = nameof(TowerDatabaseAsset), menuName = "Idle-Defense/Entities/Tower/Database")]
    public class TowerDatabaseAsset : ScriptableObject
    {
        private readonly Dictionary<int, int> _idToIndexMap = new();

        [SerializeField]
        private ConfigAssetRef[] _entries;

        public ReadOnlyMemory<ConfigAssetRef> Entries
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
            entry = null;
            return false;
        }
    }
}
