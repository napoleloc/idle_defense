using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Entities.Characters.Enemy.AI.Minion;
using Module.GameCommon;
using UnityEngine;

namespace Module.Entities.Characters.Enemy.Pooling
{
    public class MinionPoolManager : EnemyPoolManager<MinionId, MinionAIController>
    {
        public static readonly Id<MinionPoolManager> PresetId = default;

        [SerializeField]
        private MinionId[] _prepooledMinionIds;

        private ReadOnlyMemory<MinionId> PrepooledMinionIds
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _prepooledMinionIds;
        }

        public override async UniTask InitializeAsync()
        {
            var ids = PrepooledMinionIds;

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
