using System;
using Module.Core.HFSM;
using Module.Core.HFSM.Transitions;
using Module.Entities.Characters.Hero.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Characters.Hero
{
    public class MonoHeroBehaviourComponent : MonoBehaviour, IEntityComponent
    {
        private readonly StateMachine<HeroState, HeroStateEvent> _stateMachine = new();

        [HorizontalGroup("Inspector",order:1, width: 0.5F)]
        [TabGroup("Inspector/Rotation Control", "Rotation Control", order: 1)]
        [PropertyRange(0, 1)]
        [LabelText("Damping")]
        [SerializeField]
        private float _rotationSmoothTimeWithoutTarget = 0.15F;

        [TabGroup("Inspector/Debugging", "Debugging", order: 2)]
        [SerializeField, ReadOnly]
        private HeroState _currentState;

        private MonoHeroController _controller;
        private MonoHeroAttributeComponent _attributeComponent;
        private MonoHeroTargetFindingComponent _targetFindingComponent;

        private float _rotationVelocity;
        private TimeSpan _normalAttackCountdown;
        private TimeSpan _specialAttackCountdown;

        public void InitializeDependencies()
        {
            _controller = GetComponent<MonoHeroController>();
            _attributeComponent = GetComponent<MonoHeroAttributeComponent>();
            _targetFindingComponent = GetComponent<MonoHeroTargetFindingComponent>();
        }

        public void InitializeComponent()
        {
            _normalAttackCountdown = TimeSpan.Zero;
            _specialAttackCountdown = TimeSpan.Zero;

            InitStateMachineInternal();
        }

        public void UpdateComponent()
        {
            UpdateStateMachineInternal();

            if(_currentState == HeroState.Idle)
            {
                UpdateRotationInternal();
            }
        }

        private void InitStateMachineInternal()
        {
            // Declare states
            _stateMachine.AddState(HeroState.Appear, new HeroAppearState(_controller, true));
            _stateMachine.AddState(HeroState.Idle, new HeroIdleState(_controller, true));
            _stateMachine.AddState(HeroState.NormalAttack, new HeroNormalAttackState(_controller, true));
            _stateMachine.AddState(HeroState.SpecialAttack, new HeroSpecialAttackState(_controller, true));

            InitTransitionConditionInternal();

            _stateMachine.Initialize();
        }

        private void InitTransitionConditionInternal()
        {
            _stateMachine.AddTransition(HeroState.Appear, HeroState.Idle);

            _stateMachine.AddTransition(HeroState.Idle, HeroState.NormalAttack, CanChangeToNormalAttackState);
            _stateMachine.AddTransition(HeroState.NormalAttack, HeroState.Idle);

        }

        private bool CanChangeToNormalAttackState(TransitionCondition<HeroState> transition)
        {
            _normalAttackCountdown += TimeSpan.FromSeconds(Time.deltaTime);

            if (_targetFindingComponent.HasTarget
                && _normalAttackCountdown.TotalSeconds >= _attributeComponent.NormalAttackInterval)
            {
                _normalAttackCountdown = TimeSpan.Zero;
                return true;
            }

            return false;
        }

        private bool CanChangeToSpecialAttackState(TransitionCondition<HeroState> transition)
        {
            if (_targetFindingComponent.HasTarget
                && _specialAttackCountdown.TotalMilliseconds >= _attributeComponent.SpecialAttackInterval)
            {
                _specialAttackCountdown = TimeSpan.Zero;
                return true;
            }

            return false;
        }

        private void UpdateStateMachineInternal()
        {
            _stateMachine.OnLogic();

            _currentState = _stateMachine.ActiveStateName;
        }

        private void UpdateRotationInternal()
        {
            if(_targetFindingComponent.HasTarget == false)
            {
                return;
            }

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
