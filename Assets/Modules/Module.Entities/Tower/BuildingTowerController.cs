using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class BuildingTowerController : MonoBehaviour
    {
        public static readonly Id<BuildingTowerController> PresetId = default;

        private TowerAttributeComponent _attributeComponent;
        private TowerUpgradeComponent _upgradeComponent;

        private bool _initialized;

        private void Awake()
        {
            _attributeComponent = GetComponent<TowerAttributeComponent>();
            _upgradeComponent = GetComponent<TowerUpgradeComponent>();
        }

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
            await UniTask.NextFrame();

            OnInitialize();
        }

        private void OnInitialize()
        {
            _initialized = true;

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }
    }
}
