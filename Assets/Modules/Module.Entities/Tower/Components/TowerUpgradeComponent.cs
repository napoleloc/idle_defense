using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.PubSub;
using Module.Core.Extended.PubSub;
using Module.Entities.Tower.PubSub;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class TowerUpgradeComponent : MonoBehaviour, IEntityComponent
    {
        private readonly List<ISubscription> _subscriptions = new List<ISubscription>();

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private uint _level;

        private TowerAttributeComponent _attributeComponent;

        public void InitializeDependencies()
        {
            _attributeComponent = GetComponent<TowerAttributeComponent>();
        }

        public void Initialize()
        {
            var subscriber = WorldMessenger.Subscriber.TowerScope().WithState(this);
            subscriber.Subscribe<UpgradeTowerMessage>(static (state, msg) => state.Handle(msg)).AddTo(_subscriptions);
            subscriber.Subscribe<CancelTowerUpgradeMessage>(static (state, msg) => state.Handle(msg)).AddTo(_subscriptions);
        }

        public void Deinitialize()
        {
            _subscriptions.Unsubscribe();
        }

        private void Handle(UpgradeTowerMessage message)
        {
            _level = message.Level;
        }

        private void Handle(CancelTowerUpgradeMessage message)
        {

        }
    }
}
