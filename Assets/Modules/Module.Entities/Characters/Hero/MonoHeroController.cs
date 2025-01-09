using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Entities.Characters.Enemy;
using UnityEngine;

namespace Module.Entities.Characters.Hero
{
    public class MonoHeroController : MonoBehaviour
    {
        public readonly static Id<MonoHeroController> PresetId = default;

        // Components
        private MonoHeroAttributeComponent _attributeComponent;
        private MonoHeroInventoryComponent _inventoryComponent;
        private MonoHeroTargetFindingComponent _targetFindingComponent;
        private MonoHeroBehaviourComponent _behaviourComponent;

        private bool _initialized;

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
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private void Update()
        {
            if (_initialized)
            {
                UpdateComponents();
            }
        }

        private void InitializeDependencies()
        {
            _attributeComponent = GetComponent<MonoHeroAttributeComponent>();
            _inventoryComponent = GetComponent<MonoHeroInventoryComponent>();
            _targetFindingComponent = GetComponent<MonoHeroTargetFindingComponent>();
            _behaviourComponent = GetComponent<MonoHeroBehaviourComponent>();

            _behaviourComponent.InitializeDependencies();
        }

        private async UniTask InitializeAsync()
        {
            await WaitInitialize();

            OnInitialize();
        }

        private void OnInitialize()
        {
            _targetFindingComponent.Initialize();

            _initialized = true;

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        public async UniTask WaitInitialize()
        {
            await GlobalValueVault<bool>.WaitUntil(WorldEnemy.PresetId, true);
        }

        private void UpdateComponents()
        {
            _targetFindingComponent.UpdateComponent();
            _behaviourComponent.UpdateComponent();
        }
    }
}
