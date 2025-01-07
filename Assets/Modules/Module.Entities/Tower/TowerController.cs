using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class TowerController : MonoBehaviour, IEntityComponent
    {
        public static readonly Id<TowerController> PresetId = default;

        [BoxGroup("Debugging", centerLabel: true)]
        [SerializeField, ReadOnly]
        private bool _initialized;

        // Components
        private TowerAttributeComponent _attributeComponent;
        private TowerTargetFindingComponent _targetFindingComponent;
        private TowerUpgradeComponent _upgradeComponent;

        private void Awake()
        {
            InitializeDependencies();
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
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
            _upgradeComponent = GetComponent<TowerUpgradeComponent>();
        }

        private void Initialize()
        {
            OnInitialize();
        }

        private void OnInitialize()
        {
            _initialized = true;

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void UpdateComponents()
        {
            _targetFindingComponent.UpdateComponent();
        }
    }
}
