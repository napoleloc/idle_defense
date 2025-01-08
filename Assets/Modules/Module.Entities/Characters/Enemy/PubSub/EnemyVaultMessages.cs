using EncosyTower.Modules;
using EncosyTower.Modules.PubSub;
using Module.Entities.Characters.Enemy.AI;
using UnityEngine;

namespace Module.Entities.Characters.Enemy.PubSub
{
    public readonly struct RegisterEnemyMessage : IMessage
    {
        public readonly Id Id;
        public readonly EnemyAIController Enemy;

        public RegisterEnemyMessage(Id id, EnemyAIController enemy)
        {
            Id = id;
            Enemy = enemy;
        }
    }

    public readonly struct UnregisterEnemyMessage : IMessage
    {
        public readonly Id Id;
        
        public UnregisterEnemyMessage(Id id)
        {
            Id = id;
        }
    }
}
