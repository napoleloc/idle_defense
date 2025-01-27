using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.Camera;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class BuildingTowerController : MonoBehaviour
    {
        public static readonly Id<BuildingTowerController> PresetId = default;

        private bool _initialized;

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
            await GlobalValueVault<bool>.WaitUntil(WorldCamera.PresetId, true);

            OnInitialize();
        }

        private void OnInitialize()
        {
            if(GlobalObjectVault.TryGet(WorldCamera.PresetId, out var worldCamera))
            {
                worldCamera.Cameras.Span[0].Follow = transform;
            }

            _initialized = true;

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }
    }
}
