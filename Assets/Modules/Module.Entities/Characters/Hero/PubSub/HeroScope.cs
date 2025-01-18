using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.PubSub;

namespace Module.Entities.Characters.Hero.PubSub
{
    public readonly struct HeroScope : IEquatable<HeroScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(HeroScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is HeroScope;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(HeroScope);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(HeroScope _, HeroScope __)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HeroScope _, HeroScope __)
            => false;
    }

    public static class MessengerHeroScopeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageSubscriber.Subscriber<HeroScope> HeroScope(this MessageSubscriber self)
            => self.Scope<HeroScope>(default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessagePublisher.Publisher<HeroScope> HeroScope(this MessagePublisher self)
            => self.Scope<HeroScope>(default);
    }
}
