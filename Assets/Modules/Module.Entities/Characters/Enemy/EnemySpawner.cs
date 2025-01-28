using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Entities.Characters.Enemy.Pooling;
using UnityEngine;

namespace Module.Entities.Characters.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public static readonly Id<EnemySpawner> PresetId = default;

        private MinionPoolManager _minionAIPoolManager;
        private ElitePoolManager _eliteAIPoolManager;

        private async void Start()
        {
            await InitializeAsync();
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private async UniTask InitializeAsync()
        {
            await GlobalValueVault<bool>.WaitUntil(MinionPoolManager.PresetId, true);
            await GlobalValueVault<bool>.WaitUntil(ElitePoolManager.PresetId, true);

            GlobalObjectVault.TryGet(MinionPoolManager.PresetId, out _minionAIPoolManager);
            GlobalObjectVault.TryGet(ElitePoolManager.PresetId, out _eliteAIPoolManager);

            _minionAIPoolManager.Scene = gameObject.scene;
            _eliteAIPoolManager.Scene = gameObject.scene;

            await _minionAIPoolManager.InitializeAsync();
            await _eliteAIPoolManager.InitializeAsync();

            OnInitialize();
        }

        private void OnInitialize()
        {
            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }
    }
}
