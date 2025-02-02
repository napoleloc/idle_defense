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
        private readonly Dictionary<RuntimeTalentIdData, int> _idToIndex = new();
        private readonly FasterList<RuntimeTalentDataEntry> _unlockedTalents = new();

        public ReadOnlyMemory<RuntimeTalentDataEntry> UnlockedTalents
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _unlockedTalents.AsReadOnlyMemory();
        }

        public int UnlockedCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _unlockedTalents.Count;
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

            // Kiểm tra tồn tại nhanh
            if (_idToIndex.TryGetValue(id, out var index))
            {
                // Cập nhật dữ liệu cũ
                var oldTalent = _unlockedTalents[index];
                var oldId = oldTalent.Id;
                if (oldId.Kind != kind)
                {
                    // Xóa khỏi danh sách Kind cũ nếu loại thay đổi
                    if (_unlockedTalentsByKind.TryGetValue(oldId.Kind, out var oldList))
                    {
                        oldList.Remove(oldTalent);
                    }
                }

                // Cập nhật talent mới
                _unlockedTalents[index] = talent;
            }
            else
            {
                // Thêm mới
                index = _unlockedTalents.Count;
                _unlockedTalents.Add(talent);
                _idToIndex[id] = index;
            }

            // Thêm vào _unlockedTalentsByKind
            if (_unlockedTalentsByKind.TryGetValue(kind, out var kindList) == false)
            {
                kindList = new FasterList<RuntimeTalentDataEntry>();
                _unlockedTalentsByKind[kind] = kindList;
            }

            // Kiểm tra trùng lặp trước khi thêm
            if (kindList.Contains(talent) == false)
            {
                kindList.Add(talent);
            }
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

        public bool TryGetUnlockedTalent(RuntimeTalentIdData id, out RuntimeTalentDataEntry talent)
        {
            var result = _idToIndex.TryGetValue(id, out var index);
            talent = result ? UnlockedTalents.Span[index] : default;
            return result;
        }
    }
}
