using EncosyTower.Modules.PubSub;

namespace Module.Entities.Tower.PubSub
{
    public readonly struct LoadTowerMessage : IMessage
    {
        public readonly ushort Id;

        public LoadTowerMessage(ushort id)
        {
            Id = id;
        }
    }

    public readonly struct UnloadTowerMessage : IMessage { }
    
}
