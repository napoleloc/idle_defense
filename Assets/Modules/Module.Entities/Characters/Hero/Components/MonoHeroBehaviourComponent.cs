using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Characters.Hero
{
    public class MonoHeroBehaviourComponent : MonoBehaviour, IEntityComponent
    {
        [SerializeField]
        private float _rotationSmoothTimeWithoutTarget = 5;
        [SerializeField]
        private float _attackInterval = 1;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private HeroState _currentState;

        private CharacterAnimationComponent _characterAnimationComponent;
        private MonoHeroTargetFindingComponent _targetFindingComponent;

        private float _checkAttackNextTime;
        private float _rotationVelocity;

        public void InitializeDependencies()
        {
            _characterAnimationComponent = GetComponentInChildren<CharacterAnimationComponent>();
            _targetFindingComponent = GetComponent<MonoHeroTargetFindingComponent>();

            _characterAnimationComponent.InitializeDependencies();
            _characterAnimationComponent.Initialize();
        }

        public void UpdateComponent()
        {
            AttackBehaviourInternal();
        }

        private void AttackBehaviourInternal()
        {
            if (_targetFindingComponent.HasTarget)
            {
                UpdateRotation();

                if(_currentState == HeroState.Idle)
                {
                    _characterAnimationComponent.CrossFadeAnim(GameCommon.Animation.CharAnim.NormalAttack);
                    _currentState = HeroState.Attack;
                    _targetFindingComponent.TargetEnemy.TriggerDying();

                    return;
                }

                if(_currentState == HeroState.Attack)
                {
                    if(Time.time > _checkAttackNextTime + _attackInterval)
                    {
                        _currentState = HeroState.Idle;
                    }
                }
            }
        }

        private void UpdateRotation()
        {
            Vector3 direction = (_targetFindingComponent.TargetEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float smoothAngle = Mathf.SmoothDampAngle(
                        transform.eulerAngles.y
                        , angle
                        , ref _rotationVelocity
                        , _rotationSmoothTimeWithoutTarget);

            transform.rotation = Quaternion.Euler(0.0F, smoothAngle, 0.0F);
        }
    }
}
