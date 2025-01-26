using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.PubSub;

namespace Module.Core.Extended.Audio.PubSub
{
    public readonly struct AudioScope : IEquatable<AudioScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(AudioScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is AudioScope;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(AudioScope);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(AudioScope left, AudioScope right)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(AudioScope left, AudioScope right)
            => false;
    }

    public static class MessengerAudioScopeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageSubscriber.Subscriber<AudioScope> AudioScope(this MessageSubscriber self)
            => self.Scope<AudioScope>(default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessagePublisher.Publisher<AudioScope> AudioScope(this MessagePublisher self)
            => self.Scope<AudioScope>(default);
    }
}
