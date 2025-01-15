using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Module.Worlds.BattleWorld.Attribute.Modifiers;
using UnityEngine;

namespace Module.Worlds.BattleWorld.Attribute
{
    public class AttributeSystem : IDisposable
    {
        private readonly Dictionary<AttributeType, Attribute> _typeToAttributeMap = new();

        public void Add(AttributeType type, Attribute attribute)
        {
            var map = _typeToAttributeMap;
            
            if(map.ContainsKey(type) == false)
            {
                map[type] = attribute;
                return;
            }
        }

        public void Add(AttributeType type , float value)
        {
            var map = _typeToAttributeMap;

            if(map.ContainsKey(type) == false)
            {
                var attribute = new Attribute(value);
                map[type] = attribute;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(AttributeType type, out Attribute attribute)
            => _typeToAttributeMap.TryGetValue(type, out attribute);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Modify(AttributeType type, int scope, Modifier modifier)
        {
            if(_typeToAttributeMap.TryGetValue(type, out var attribute))
            {
                attribute.Add(scope, modifier);
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Modify(AttributeType type, Modifier modifier)
        {
            if (_typeToAttributeMap.TryGetValue(type, out var attribute))
            {
                attribute.Add(modifier);
                return;
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool TryRemove(AttributeType type)
            => _typeToAttributeMap.Remove(type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRemoveModifier(AttributeType type, int scope, Modifier modifier)
        {
            var result = _typeToAttributeMap.TryGetValue(type, out var attribute);
            return result ? attribute.TryRemove(scope, modifier) : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRemoveModifier(AttributeType type, Modifier modifier)
        {
            var result = _typeToAttributeMap.TryGetValue(type, out var attribute);
            return result ? attribute.TryRemove(modifier) : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveModifiersByScope(AttributeType type, int scope)
        {
            if(_typeToAttributeMap.TryGetValue(type, out var attribute))
            {
                attribute.RemoveByScope(scope);
            }
        }

        public void Dispose()
        {
            var map = _typeToAttributeMap;

            foreach (var attribute in map.Values)
            {
                attribute.Dispose();
            }

            map.Clear();
        }
    }
}
