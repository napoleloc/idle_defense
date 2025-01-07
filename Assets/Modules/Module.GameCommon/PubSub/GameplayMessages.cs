using EncosyTower.Modules.PubSub;

namespace Module.GameCommon.PubSub
{
    public readonly struct StartGameMessage : IMessage { }
    public readonly struct PauseGameMessage : IMessage { }
    public readonly struct UnpauseGameMessage : IMessage { }
    public readonly struct QuitGameMessage : IMessage { }

    public readonly struct StageCompleteMessage : IMessage { }
    public readonly struct StageFailedMessage : IMessage { }
}
