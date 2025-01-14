using System;

namespace Module.Worlds.BattleWorld.Attribute.Modifiers.Internals
{
    internal interface IModifiersOperation : IDisposable
    {
        /// <summary>Adds a modifier to the collection.</summary>
        /// <param name="modifier">The modifier to add.</param>
        void Add(Modifier modifier);

        /// <summary>Adds a modifier to the collection.</summary>
        /// <param name="modifier">The modifier to add.</param>
        void Add(int scope, Modifier modifier);

        void Add(int scope, Span<Modifier> modifiers);

        /// <summary>Attempts to remove a specific modifier from the collection.</summary>
        /// <param name="modifier">The modifier to remove.</param>
        /// <returns><c>true</c> if the modifier was successfully removed; otherwise, <c>false</c>.</returns>
        bool TryRemove(Modifier modifier);

        /// <summary>Attempts to remove a specific modifier from the collection.</summary>
        /// <param name="modifier">The modifier to remove.</param>
        /// <returns><c>true</c> if the modifier was successfully removed; otherwise, <c>false</c>.</returns>
        bool TryRemove(int scope, Modifier modifier);

        bool TryRemove(int scope);

        /// <summary>Calculates the total value of the modifiers applied to a base value.</summary>
        /// <param name="baseValue">The base value of the stat before any modifiers are applied.</param>
        /// <param name="currentValue">The current value of the stat after previous modifiers have been applied.</param>
        /// <returns>The calculated value of the stat after all modifiers have been applied.</returns>
        float CalculateModifiersValue(float baseValue, float currentValue);

    }
}
