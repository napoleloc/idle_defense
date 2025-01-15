using System.Runtime.CompilerServices;
using Module.Worlds.BattleWorld.Attribute;
using UnityEngine;

namespace Module.Entities.Characters.Hero
{
    public class MonoHeroAttributeComponent : MonoBehaviour, IEntityComponent, IDeinitialization
    {
        private AttributeSystem _attributeSystem;

        public void InitializeDependencies()
        {
            _attributeSystem = new();
        }

        public void Deinitialize()
        {
            _attributeSystem.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetValue(AttributeType type)
        {
            var result = _attributeSystem.TryGet(type, out var attribute);
            return result ? attribute.Value : 0;
        }
    }
}
