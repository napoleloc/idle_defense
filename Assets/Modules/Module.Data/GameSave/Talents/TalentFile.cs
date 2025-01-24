using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Logging;
using Module.Data.Runtime.Talents;
using Module.GameCommon.Save;

namespace Module.Data.GameSave.Talents
{
    [System.Serializable]
    public class TalentFile : IFile
    {
        private FasterList<TalentEntry> _talentsToUnlock;

        public TalentFile()
        {
            _talentsToUnlock = new();
        }

        public int UnlockedCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _talentsToUnlock.Count;
        }

        public ReadOnlyMemory<TalentEntry> TalentsToUnlock
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _talentsToUnlock.AsReadOnlyMemory();
        }

        public void AddEntry(TalentEntry entry)
            => _talentsToUnlock.Add(entry);

        public void AddEntries(ReadOnlySpan<TalentEntry> entries)
            => _talentsToUnlock.AddRange(_talentsToUnlock);

        public void GetEntries(Span<TalentEntry> entries)
        {
            var lenght = _talentsToUnlock.Count;

            if(lenght <= 0)
            {
                DevLoggerAPI.LogError("");
                return;
            }

            _talentsToUnlock.CopyTo(entries);

            _talentsToUnlock.RemoveAt(0, lenght);
        }

        public void Flush() { }
    }
}
