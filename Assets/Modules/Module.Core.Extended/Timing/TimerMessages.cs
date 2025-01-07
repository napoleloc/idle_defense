using EncosyTower.Modules.PubSub;
using EncosyTower.Modules;
using System.Runtime.CompilerServices;

namespace Module.Core.Extended.Timing
{
    public readonly struct RegisterToTimerMessage : IMessage
    {
        public readonly Id2 Id;
        public readonly ITimer Timer;

        public RegisterToTimerMessage(Id2 id, ITimer timer)
        {
            Id = id;
            Timer = timer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RegisterToTimerMessage From<T>(T timer) where T : ITimer
            => new(TypeCache.GetId<T>().ToId2(), timer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RegisterToTimerMessage From<T>(Id id, T timer) where T : ITimer
            => new(TypeCache.GetId<T>().ToId2(id), timer);
    }

    public readonly struct UnregisterToTimerMessage : IMessage
    {
        public readonly Id2 Id;

        public UnregisterToTimerMessage(Id2 id)
        {
            Id = id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnregisterToTimerMessage From<T>() where T : ITimer
            => new(TypeCache.GetId<T>().ToId2());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnregisterToTimerMessage From<T>(Id id) where T : ITimer
            => new(TypeCache.GetId<T>().ToId2(id));
    }

    public readonly record struct SetCurrentTimeMessage(long ServerTimeDeltaInSeconds) : IMessage;

    public readonly record struct StopUpdateTimeMessage(bool IsStop) : IMessage;
}
