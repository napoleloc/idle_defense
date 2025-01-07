using EncosyTower.Modules.PubSub;

namespace Module.Entities.Tower.PubSub
{
    public readonly struct UpgradeTowerMessage : IMessage
    {
        public readonly ushort Id;
        public readonly uint Level;

        public UpgradeTowerMessage(ushort id, uint level)
        {
            Id = id;
            Level = level;
        }
    }

    public readonly struct CancelTowerUpgradeMessage : IMessage
    {
        public readonly ushort Id;

        public CancelTowerUpgradeMessage(ushort id)
        {
            Id = id;
        }
    }
}
