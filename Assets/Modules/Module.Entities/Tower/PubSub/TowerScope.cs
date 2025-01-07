using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.PubSub;

namespace Module.Entities.Tower.PubSub
{
    public readonly struct TowerScope : IEquatable<TowerScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TowerScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(TowerScope);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(TowerScope _, TowerScope __)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(TowerScope _, TowerScope __)
            => false;
    }

    public static class MessengerTowerScopeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageSubscriber.Subscriber<TowerScope> TowerScope(this MessageSubscriber self)
            => self.Scope<TowerScope>(default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessagePublisher.Publisher<TowerScope> TowerScope(this MessagePublisher self)
            => self.Scope<TowerScope>(default);
    }
}
