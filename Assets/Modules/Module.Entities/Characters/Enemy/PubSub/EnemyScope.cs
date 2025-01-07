using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.PubSub;
using JetBrains.Annotations;

namespace Module.Entities.Characters.Enemy.PubSub
{
    public readonly struct EnemyScope : IEquatable<EnemyScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(EnemyScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is EnemyScope;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(EnemyScope);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(EnemyScope _, EnemyScope __)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(EnemyScope _, EnemyScope __)
            => false;
    }

    public static class MessengerEnemyScopeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageSubscriber.Subscriber<EnemyScope> EnemyScope([NotNull] this MessageSubscriber self)
            => self.Scope<EnemyScope>(default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessagePublisher.Publisher<EnemyScope> EnemyScope([NotNull] this MessagePublisher self)
            => self.Scope<EnemyScope>(default);
    }
}
