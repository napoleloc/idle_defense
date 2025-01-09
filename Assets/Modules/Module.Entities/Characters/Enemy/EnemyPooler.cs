using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Entities.Characters.Enemy.Builder;
using Module.Entities.Characters.Hero;
using Module.Entities.Tower;
using Module.GameCommon;
using UnityEngine;

namespace Module.Entities.Characters.Enemy
{
    public class EnemyPooler : MonoBehaviour
    {
        public static readonly Id<EnemyPooler> PresetId = default;

        private MinionBuilder _minionBuilder;
        private EliteBuilder _eliteBuilder;

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
            _minionBuilder = new MinionBuilder();
            _eliteBuilder = new EliteBuilder();

            await GlobalValueVault<bool>.WaitUntil(BuildingTowerController.PresetId, true);
            await GlobalValueVault<bool>.WaitUntil(MonoHeroController.PresetId, true);
            await _minionBuilder.InitializePool(gameObject.scene);
            await _eliteBuilder.InitializePool(gameObject.scene);

            OnInitialize();
        }

        private void OnInitialize()
        {
            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        public void GetFromPoolBy(EnemyType type, Vector3 position, Vector3 rotation)
        {
            var instance = type switch{
                EnemyType.Minion =>  _minionBuilder.GetFromPool(MinionId.minion_1, false),
                EnemyType.Elite => _eliteBuilder.GetFromPool(EliteId.elite_1, false),
                _ => default,
            };

            if (instance.IsValid())
            {
                instance.transform.position = position;
                instance.transform.rotation = Quaternion.LookRotation(rotation, Vector3.up);
                instance.SetActive(true);
                if (instance.TryGetComponent<IEntityPoolable>(out var poolable))
                {
                    poolable.OnGetFromPool();
                }
            }
        }

        public void ReturnToPoolBy(EnemyType type, GameObject instance)
        {
            switch (type)
            {
                case EnemyType.Minion:
                    _minionBuilder.ReturnToPool(MinionId.minion_1, instance);
                    break;
                case EnemyType.Elite:
                    _eliteBuilder.ReturnToPool(EliteId.elite_1, instance);
                    break;
            }
        }
    }
}
