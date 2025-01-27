using EncosyTower.Modules.PubSub;
using Module.Entities.Tower.Data;

namespace Module.Entities.Tower.PubSub
{
    public readonly struct SpawnTowerMessage : IMessage
    {
        public readonly TowerIdConfig Id;

        public SpawnTowerMessage(TowerIdConfig id)
        {
            Id = id;
        }
    }

    public readonly struct ReleaseTowerMessage : IMessage { }
    
}
