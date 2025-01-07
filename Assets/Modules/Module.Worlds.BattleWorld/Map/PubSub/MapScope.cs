using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.PubSub;

namespace Module.Worlds.BattleWorld.Map.PubSub
{
    public readonly struct MapScope : IEquatable<MapScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(MapScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(MapScope);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(MapScope _, MapScope __)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(MapScope _, MapScope __)
            => false;
    }

    public static class MessengerMapScopeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageSubscriber.Subscriber<MapScope> MapScope(this MessageSubscriber seft)
            => seft.Scope<MapScope>(default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessagePublisher.Publisher<MapScope> MapScope(this MessagePublisher seft)
            => seft.Scope<MapScope>(default);
    }
}
