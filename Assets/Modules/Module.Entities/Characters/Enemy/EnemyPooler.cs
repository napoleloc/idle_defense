using EncosyTower.Modules;
using EncosyTower.Modules.Logging;
using EncosyTower.Modules.Vaults;
using Module.Entities.Characters.Enemy.Builder;
using Module.GameCommon;
using UnityEngine;

namespace Module.Entities.Characters.Enemy
{
    public class EnemyPooler : MonoBehaviour
    {
        public static readonly Id<EnemyPooler> PresetId = default;

        [SerializeField]
        private EnemyBuildingConfigAsset _buildingConfig;

        private MinionBuilder _minionBuilder;
        private EliteBuilder _eliteBuilder;

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private void Initialize()
        {
            var configs = _buildingConfig.BuildingConfigs.Span;

            for (int i = 0; i < configs.Length; i++)
            {
                var config = configs[i];

                switch (config.Type)
                {
                    case GameCommon.EnemyType.Minion:
                    {
                        _minionBuilder = MinionBuilder.CreateInstance(config, transform);
                        break;
                    }

                    case GameCommon.EnemyType.Elite:
                    {
                        _eliteBuilder = EliteBuilder.CreateInstance(config, transform);
                        break;
                    }
                }
            }

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
            }
        }
    }
}
