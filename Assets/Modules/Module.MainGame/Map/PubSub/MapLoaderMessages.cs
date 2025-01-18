using EncosyTower.Modules.PubSub;

namespace Module.MainGame.Map
{
    public readonly struct InitDataMessage : IMessage
    {
        public readonly ushort MapId;
        public readonly ushort Region;
    }

    public readonly struct LoadMapMessage : IMessage { }
    public readonly struct UnloadMapMessage : IMessage { }
}
