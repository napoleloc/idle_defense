using EncosyTower.Modules.Logging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class TowerWeaponComponent : MonoBehaviour, IEntityComponent
    {
        [Title("Settings", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private float _attackInterval = 1;
        [SerializeField]
        private float _damage;
        
        private TowerTargetFindingComponent _targetFindingComponent;

        private float _checkDealDamageNextTime;

        public void InitializeDependencies()
        {
            _targetFindingComponent = GetComponent<TowerTargetFindingComponent>();
        }

        public void UpdateComponent()
        {
            if (_targetFindingComponent.HasTarget)
            {
                if (CanDealDamage())
                {
                    DealDamageToEnemy();
                    _checkDealDamageNextTime = Time.time;
                }
            }
        }

        private bool CanDealDamage()
            => Time.time >= _checkDealDamageNextTime + _attackInterval;

        private void DealDamageToEnemy()
            => DevLoggerAPI.LogInfo($"Deal damage {_damage} to {_targetFindingComponent.TargetEnemy.name}");
    }
}
