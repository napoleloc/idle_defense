using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine.Serialization;
using UnityEngine;

namespace Module.Worlds.BattleWorld.Attribute.Modifiers
{
    public readonly struct Modifier : IEquatable<Modifier>, IComparable<Modifier>
    {
        public readonly ushort Id;

        public float Value { readonly get; init; }
        public object Source { readonly get; init; }
        public ModifierType Type { readonly get; init; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Modifier other)
           => Id == other.Id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is Modifier other && other.Id == Id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => Id.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => $"Value:{Value.ToString(CultureInfo.InvariantCulture)} Type:{Type} Source object: {Source ?? "None"}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Modifier other)
            => (int)other.Id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Modifier left, Modifier right)
            => left.Value == right.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Modifier left, Modifier right)
            => left.Value != right.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator float(Modifier modifier) 
            => modifier.Value;

        [Serializable]
        public struct Serializable
        {
            [FormerlySerializedAs("_id")]
            [SerializeField]
            private ushort _id;

            public ushort Id
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _id;
            }
        }
    }

}
