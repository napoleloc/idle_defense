using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using Module.Worlds.BattleWorld.Attribute.Modifiers;
using Module.Worlds.BattleWorld.Attribute.Modifiers.Internals;

namespace Module.Worlds.BattleWorld.Attribute
{
    public class Attribute : IDisposable
    {
        private const int MAXIMUM_ROUND_DIGITS = 6;

        private readonly ArrayMap<ModifierType, IModifiersOperation> _typeToOperationMap = new();

        private float _baseValue;
        private float _currentValue;
        private int _digitAccuracy;
        private bool _isDirty;

        public Attribute() { }

        public Attribute(float baseValue)
        {
            _currentValue = baseValue;
            _digitAccuracy = MAXIMUM_ROUND_DIGITS;
            _isDirty = false;

            _typeToOperationMap[ModifierType.Flat] = new FlatModifierOperation();
            _typeToOperationMap[ModifierType.Additive] = new AdditiveModifierOperation();
            _typeToOperationMap[ModifierType.Multiplicative] = new MultiplicativeModifierOperation();
        }

        public float Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_isDirty)
                {
                    _currentValue = CalculateModifiedValueInternal(_digitAccuracy);
                }
                return _currentValue;
            }
        }

        public bool IsDirty
        {
            get => _isDirty;

            set
            {
                _isDirty = value;
            }
        }

        public void Dispose()
        {
            var map = _typeToOperationMap;

            lock (map)
            {
                var operations = map.GetValues();

                foreach (var operation in operations)
                {
                    operation.Dispose();
                }

                map.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Modifier modifier)
        {
            var type = modifier.Type;
            if (_typeToOperationMap.TryGetValue(type, out var handle))
            {
                handle.Add(modifier);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(int scope, Modifier modifier)
        {
            var type = modifier.Type;
            if(_typeToOperationMap.TryGetValue(type, out var handle))
            {
                handle.Add(scope, modifier);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRemove(Modifier modifier)
        {
            var type = modifier.Type;
            var result = _typeToOperationMap.TryGetValue(type, out var handler);
            return result ? handler.TryRemove(modifier) : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRemove(int scope, Modifier modifier)
        {
            var type = modifier.Type;
            var result = _typeToOperationMap.TryGetValue(type, out var handler);
            return result ? handler.TryRemove(scope, modifier) : false;
        }

        public void RemoveByScope(int scope)
        {
            var handlers = _typeToOperationMap.GetValues();
            for ( var i = 0; i < handlers.Length; i++)
            {
                var handler = handlers[i];
                handler.TryRemove(scope);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float CalculateModifiedValueInternal(int digitAccuracy)
        {
            digitAccuracy = Math.Clamp(digitAccuracy, 0, MAXIMUM_ROUND_DIGITS);

            var finalValue = _baseValue;

            lock (_typeToOperationMap)
            {
                var values = _typeToOperationMap.GetValues();

                for (int i = 0; i < values.Length; i++)
                {
                    finalValue += values[i].CalculateModifiersValue(_baseValue, finalValue);
                }

                IsDirty = false;

                return (float)Math.Round(finalValue, digitAccuracy);
            }
        }
    }
}
