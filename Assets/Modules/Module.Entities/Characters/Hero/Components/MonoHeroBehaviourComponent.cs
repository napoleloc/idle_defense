using Module.Core.HFSM;
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
        private float _rotationSmoothTimeWithoutTarget = 5;

        [TabGroup("Inspector/Debugging", "Debugging", order: 2)]
        [SerializeField, ReadOnly]
        private HeroState _currentState;

        private CharacterAnimationComponent _characterAnimationComponent;
        private MonoHeroController _controller;
        private MonoHeroTargetFindingComponent _targetFindingComponent;

        private float _rotationVelocity;

        public void InitializeDependencies()
        {
            _characterAnimationComponent = GetComponentInChildren<CharacterAnimationComponent>();
            _controller = GetComponent<MonoHeroController>();
            _targetFindingComponent = GetComponent<MonoHeroTargetFindingComponent>();

            _characterAnimationComponent.InitializeDependencies();
            _characterAnimationComponent.Initialize();
        }

        public void InitializeComponent()
        {
            InitStateMachineInternal();
        }

        public void UpdateComponent()
        {
            _stateMachine.OnLogic();
            UpdateRotationInternal();
        }

        private void InitStateMachineInternal()
        {
            // Declare states
            _stateMachine.AddState(HeroState.Appear, new HeroAppearState(_controller, true));
            _stateMachine.AddState(HeroState.Idle, new HeroIdleState(_controller, false));
            _stateMachine.AddState(HeroState.NormalAttack, new HeroNormalAttackState(_controller, true));
            _stateMachine.AddState(HeroState.SpecialAttack, new HeroSpecialAttackState(_controller, true));

            InitTransitionConditionInternal();

            _stateMachine.Initialize();
        }

        private void InitTransitionConditionInternal()
        {
            _stateMachine.AddTransition(HeroState.Appear, HeroState.Idle);
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
                        , _rotationSmoothTimeWithoutTarget * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0.0F, smoothAngle, 0.0F);
        }
    }
}
