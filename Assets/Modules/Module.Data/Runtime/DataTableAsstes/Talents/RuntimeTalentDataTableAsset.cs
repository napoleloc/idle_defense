using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Logging;
using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Data.Runtime.DataTableAsstes.Talents
{
    [CreateAssetMenu(fileName = nameof(RuntimeTalentDataTableAsset), menuName = "Idle-Defense/Runtime-Data/Table-Assets/Talent Data Table")]
    public class RuntimeTalentDataTableAsset : RuntimeDataTableAsset
    {
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "GetLabel")]
        [SerializeField]
        private RuntimeTalentDataEntry[] _defaultTalents;

        private readonly Dictionary<AttributeKind, FasterList<RuntimeTalentDataEntry>> _unlockedTalentsByKind = new();

        public int UnlockedCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 10;
        }

        public int UnlockedCountByKind(AttributeKind kind)
        {
            var result = _unlockedTalentsByKind.TryGetValue(kind, out var talentList);
            return result ? talentList.Count : 0;
        }

        public void AddUnlockTalent(RuntimeTalentDataEntry talent)
        {
            var id = talent.Id;
            var kind = id.Kind;
            

        }

        public bool TryGetUnlockedTalentsByKind(AttributeKind kind, Span<RuntimeTalentDataEntry> talentsUnlocked)
        {
            if (_unlockedTalentsByKind.TryGetValue(kind, out var entries) == false)
            {
                DevLoggerAPI.LogError("");
                talentsUnlocked = default;
                return false;
            }

            entries.CopyTo(talentsUnlocked);
            return true;
        }
    }
}
