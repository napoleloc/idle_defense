using EncosyTower.Modules.PubSub;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Module.Core.Extended.Timing
{
    public readonly struct TimerScope : IEquatable<TimerScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TimerScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is TimerScope;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(TimerScope);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(TimerScope left, TimerScope right)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(TimerScope left, TimerScope right)
            => false;
    }

    public static class MessengerTimerScopeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageSubscriber.Subscriber<TimerScope> TimerScope([NotNull] this MessageSubscriber self)
            => self.Scope<TimerScope>(default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessagePublisher.Publisher<TimerScope> TimerScope([NotNull] this MessagePublisher self)
            => self.Scope<TimerScope>(default);
    }
}
