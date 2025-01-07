using EncosyTower.Modules.PubSub;

namespace Module.Worlds.BattleWorld.Map.PubSub
{
    public readonly struct InitDataMessage : IMessage
    {
        public readonly ushort MapId;
        public readonly ushort Region;
    }

    public readonly struct LoadMapMessage : IMessage { }
    public readonly struct UnloadMapMessage : IMessage { }
}
