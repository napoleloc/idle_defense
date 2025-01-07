using EncosyTower.Modules.PubSub;

namespace Module.Core.Extended.Audio.PubSub
{
    public readonly struct PlaySoundMessage : IMessage
    {
        
    }

    public readonly struct PauseSoundMessage : IMessage { }
    public readonly struct UnpauseSoundMessage : IMessage { }
    public readonly struct StopSoundMessage : IMessage { }
}
