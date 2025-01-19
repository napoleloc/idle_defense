using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.PubSub;
using Module.Core.Extended.PubSub;
using Module.Entities.Characters.Hero.PubSub;
using Module.Worlds.BattleWorld.Attribute;
using UnityEngine;

namespace Module.Entities.Characters.Hero
{
    public class MonoHeroAttributeComponent : MonoBehaviour, IEntityComponent, IDeinitialization
    {
        private readonly List<ISubscription> _subscriptions = new();
        private AttributeComponent _attributeComponent;

        public void InitializeDependencies()
        {
            _attributeComponent = GetComponent<AttributeComponent>();

            var subscriber = WorldMessenger.Subscriber.HeroScope().WithState(this);
            subscriber.Subscribe<UpgradeAttributeMessage>(static (state, msg) => state.Handle(msg)).AddTo(_subscriptions);
        }

        public void Deinitialize()
        {
            _attributeComponent.Dispose();
            _subscriptions.Unsubscribe();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetValue(AttributeType type)
        {
            var result = _attributeComponent.TryGet(type, out var attribute);
            return result ? attribute.Value : 0;
        }

        private void Handle(UpgradeAttributeMessage message)
        {
            if(_attributeComponent.TryGet(message.Type, out var attribute))
            {
                attribute.Add(message.Scope, message.Modifier);
            }
        }
    }
}
