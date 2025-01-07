using System;
using System.Runtime.CompilerServices;

namespace Module.Core.Extended.Currency.PubSub
{
    public readonly struct CurrencyScope : IEquatable<CurrencyScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CurrencyScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is CurrencyScope;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => base.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(CurrencyScope);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CurrencyScope left, CurrencyScope right)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CurrencyScope left, CurrencyScope right)
            => false;
    }
}
