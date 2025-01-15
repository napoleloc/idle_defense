using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;

namespace Module.Worlds.BattleWorld.Attribute.Modifiers.Internals
{
    internal abstract class ModifierOperation : IModifiersOperation, IDisposable
    {
        private const int DEFAULT_SCOPE = 1;

        protected readonly ArrayMap<int, FasterList<Modifier>> ScopedModifers = new();

        public bool IsEmpty => ScopedModifers.Count <= 0;

        public void Dispose()
        {
            var scopedModifiers = ScopedModifers;

            lock (scopedModifiers)
            {
                scopedModifiers.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Modifier modifier)
            => Add(DEFAULT_SCOPE, modifier);

        public void Add(int scope, Modifier modifier)
        {
            var scopedModifier = ScopedModifers;

            lock (scopedModifier)
            {
                if (scopedModifier.TryGetValue(scope, out var modifiers) == false)
                {
                    modifiers = new() { modifier };
                    scopedModifier[scope] = modifiers;
                    return;
                }

                modifiers.Add(modifier);
            }
        }

        public void Add(int scope, Span<Modifier> modifiers)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRemove(Modifier modifier)
            => TryRemove(DEFAULT_SCOPE, modifier);

        public bool TryRemove(int scope, Modifier modifier)
        {
            var scopedModifiers = ScopedModifers;

            lock (scopedModifiers)
            {
                var result = scopedModifiers.TryGetValue(scope, out var modifiers);
                return result ? modifiers.Remove(modifier) : result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRemove(int scope)
        {
            var result = ScopedModifers.TryGetValue(scope, out var modifiers);
            return result ? ScopedModifers.Remove(scope) : false;
        }

        public abstract float CalculateModifiersValue(float baseValue, float currentValue);
    }
}
