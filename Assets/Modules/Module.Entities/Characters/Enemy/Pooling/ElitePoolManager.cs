using System;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Entities.Characters.Enemy.AI.Elite;
using Module.GameCommon;
using UnityEngine;

namespace Module.Entities.Characters.Enemy.Pooling
{
    public class ElitePoolManager : EnemyPoolManager<EliteId, EliteAIController>
    {
        public static readonly Id<ElitePoolManager> PresetId = default;

        [SerializeField]
        private EliteId[] _prepooledEliteIds;

        public ReadOnlyMemory<EliteId> PrepooledEliteIds
        {
            get => _prepooledEliteIds;
        }

        public async override UniTask InitializeAsync()
        {
            var ids = PrepooledEliteIds;

            for (var i = 0; i < ids.Length; i++)
            {
                await PreloadAndPoolAsync(ids.Span[i], Scene);
            }
        }

        private void OnInitialize()
        {
            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }
    }
}
