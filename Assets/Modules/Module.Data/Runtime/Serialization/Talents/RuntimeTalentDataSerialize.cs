using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using Module.Data.Runtime.DataTableAsstes.Talents;

namespace Module.Data.Runtime.Serialization.Talents
{
    public class RuntimeTalentDataSerialize
    {
        private FasterList<RuntimeTalentDataEntry> _talentsToUnlock;

        public RuntimeTalentDataSerialize(ReadOnlySpan<RuntimeTalentDataEntry> defaultTalents)
        {
            if(defaultTalents.IsEmpty == false)
            {
                _talentsToUnlock.AddRange(defaultTalents, defaultTalents.Length);
            }
            else
            {
                _talentsToUnlock = new FasterList<RuntimeTalentDataEntry>();
            }
        }

        public ReadOnlyMemory<RuntimeTalentDataEntry> TalentsToUnlock
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _talentsToUnlock.AsReadOnlyMemory();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddUnlockedTalent(RuntimeTalentDataEntry talent)
            => _talentsToUnlock.Add(talent);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveUnlockedTalent(RuntimeTalentDataEntry talent)
            => _talentsToUnlock.Remove(talent);
    }
}
