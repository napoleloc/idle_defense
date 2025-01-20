using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections.Unsafe;
using Module.GameCommon.Save;
using UnityEngine;

namespace Module.Data.GameSave.Talents
{
    [System.Serializable]
    public class TalentFile : IFile
    {
        [SerializeField]
        private List<TalentEntry> _entries;

        public TalentFile()
        {
            _entries = new();
        }

        public ReadOnlyMemory<TalentEntry> Entries
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _entries.AsReadOnlyMemoryUnsafe();
        }

        public void Flush() { }
    }
}
