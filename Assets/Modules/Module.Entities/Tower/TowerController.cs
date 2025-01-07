using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.Camera;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class TowerController : MonoBehaviour, IEntityComponent
    {
        public static readonly Id<TowerController> PresetId = default;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private bool _initialized;

        // Components
        private TowerAttributeComponent _attributeComponent;
        private TowerTargetFindingComponent _targetFindingComponent;
        private TowerWeaponComponent _weaponComponent;
        private TowerUpgradeComponent _upgradeComponent;

        private WorldCamera _worldCamera;

        private void Awake()
        {
            InitializeDependencies();
        }

        private async void Start()
        {
            await InitializeAsync();
        }

        private void OnDestroy()
        {
            Deinitialize();
        }

        private void Update()
        {
            if(_initialized)
            {
                UpdateComponents();
            }
        }

        public void InitializeDependencies()
        {
            _attributeComponent = GetComponent<TowerAttributeComponent>();
            _targetFindingComponent = GetComponent<TowerTargetFindingComponent>();
            _weaponComponent = GetComponent<TowerWeaponComponent>();
            _upgradeComponent = GetComponent<TowerUpgradeComponent>();
        }

        private async UniTask InitializeAsync()
        {
            await GlobalValueVault<bool>.WaitUntil(WorldCamera.PresetId, true);
            OnInitialize();
        }

        private void OnInitialize()
        {
            if(GlobalObjectVault.TryGet(WorldCamera.PresetId, out _worldCamera))
            {
                _worldCamera.Cameras.Span[0].Follow = transform;
            }

            _attributeComponent.Initialize();
            _upgradeComponent.Initialize();

            _initialized = true;

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void Deinitialize()
        {
            if(_initialized == false)
            {
                return;
            }

            _attributeComponent.Deinitialize();
            _upgradeComponent.Deinitialize();

            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private void UpdateComponents()
        {
            _targetFindingComponent.UpdateComponent();
        }
    }
}
