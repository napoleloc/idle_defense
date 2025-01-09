using EncosyTower.Modules;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Logging;
using EncosyTower.Modules.Pooling;
using EncosyTower.Modules.PubSub;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.PubSub;
using Module.Entities.Characters.Enemy.AI;
using Module.Entities.Characters.Enemy.PubSub;
using UnityEngine;

namespace Module.Entities.Characters.Enemy
{
    public class WorldEnemy : MonoBehaviour
    {
        public static readonly Id<WorldEnemy> PresetId = default;

        private readonly ArrayMap<Id, EnemyAIController> _enemies = new();

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            _enemies.Dispose();

            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private void Initialize()
        {
            var subscriber = WorldMessenger.Subscriber.EnemyScope().WithState(this);
            subscriber.Subscribe<RegisterEnemyMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<UnregisterEnemyMessage>(static (state, msg) => state.Handle(msg));

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void Handle(RegisterEnemyMessage message)
        {
            var map = _enemies;
            map.TryAdd(message.Id, message.Enemy);
        }

        private void Handle(UnregisterEnemyMessage message)
        {
            var map = _enemies;
            map.Remove(message.Id);
        }

        public FasterList<EnemyAIController> GetEnemies()
        {
            lock (_enemies)
            {
                var list = GetEnemyList();
                var enemies = _enemies.GetValues();
                var lenght = enemies.Length;

                list.IncreaseCapacityTo(lenght);
                list.AddRange(enemies);

                return list;
            }
        }

        #region    EXTENSION_METHODS
        #endregion =================

        private static FasterList<EnemyAIController> GetEnemyList()
            => FasterListPool<EnemyAIController>.Get();
    }
}
