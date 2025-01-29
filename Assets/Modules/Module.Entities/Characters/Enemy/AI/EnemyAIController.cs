using EncosyTower.Modules;
using Module.Core.Extended.PubSub;
using Module.Core.HFSM;
using Module.Core.HFSM.States;
using Module.Entities.Characters.Enemy.PubSub;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Module.Entities.Characters.Enemy.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAIController : MonoBehaviour, IEntityComponent, IEntityPoolable
    {
        protected readonly StateMachine<EnemyState, EnemyStateEvent> StateMachine = new();

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        protected bool initialized;
        [SerializeField, ReadOnly]
        protected bool isAlive;
        [SerializeField, ReadOnly]
        private EnemyState _currentState;

        protected Id uniqueId;
        protected NavMeshAgent navMeshAgent;
        protected CharacterController characterController;
        protected CharacterPhysicsComponent characterPhysicsComponent;
        protected CharacterAnimationComponent characterAnimationComponent;

        public NavMeshAgent NavMeshAgent => navMeshAgent;
        public CharacterController CharacterController => characterController;
        public CharacterPhysicsComponent CharacterPhysicsComponent => characterPhysicsComponent;
        public CharacterAnimationComponent CharacterAnimationComponent => characterAnimationComponent;

        public bool IsAlive => isAlive;

        private void Awake()
        {
            InitializeDependencies();

            uniqueId = new(gameObject.GetInstanceID());
        }

        private void Start()
        {
            CharacterAnimationComponent.InitializeComponent();
            InitStateMachine();
            initialized = true;
          
        }

        private void Update()
        {
            if (initialized)
            {
                UpdateComponents();
                _currentState = StateMachine.ActiveStateName;
            }
        }

        private void UpdateComponents()
        {
            StateMachine.OnLogic();
        }

        public void InitializeDependencies()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            characterPhysicsComponent = GetComponent<CharacterPhysicsComponent>();
            characterAnimationComponent = GetComponentInChildren<CharacterAnimationComponent>();

            CharacterPhysicsComponent.InitializeDependencies();
            characterAnimationComponent.InitializeDependencies();

            navMeshAgent.enabled = false;
            characterPhysicsComponent.Deactivate();
        }

        public void OnGetFromPool()
        {
            isAlive = true;

            WorldMessenger.Publisher.EnemyScope()
                .Publish(new RegisterEnemyMessage(uniqueId, this)); 
        }

        public void OnReturnToPool()
        {
            StateMachine.RequestStateChange(EnemyState.Appear);

            WorldMessenger.Publisher.EnemyScope()
                .Publish(new UnregisterEnemyMessage(uniqueId));
        }

        protected virtual void InitStateMachine()
        {
            // Declare states
            StateMachine.AddState(EnemyState.Appear
                , new EnemyAppearState(this, true, OnEnterAppearState, OnExitAppearState));
            StateMachine.AddState(EnemyState.Idle
                , new EnemyIdleState(this, true));
            StateMachine.AddState(EnemyState.Chase
                , new EnemyChaseState(this, true));

            // Attack
            StateMachine.AddState(EnemyState.NormalAttack
                , new EnemyNormalAttackState(this, true, OnEnterNormalAttackState, OnExitNormalAttackState));
            StateMachine.AddState(EnemyState.SpecialAttack
                , new EnemySpecialAttackState(this, true, OnEnterSpecialAttackState, OnExitSpecialAttackState));

            // Dead
            StateMachine.AddState(EnemyState.Dying
                , new EnemyDyingState(this, true, OnEnterDyingState));
            StateMachine.AddState(EnemyState.Dead
                , new EnemyDeadState(this, true, OnEnterDeadState,0.33F));

            InitTransitionConditions();

            StateMachine.Initialize();
        }

        protected virtual void InitTransitionConditions()
        {
            StateMachine.AddTransition(EnemyState.Appear, EnemyState.Idle);
            StateMachine.AddTransition(EnemyState.Idle, EnemyState.Chase);

            StateMachine.AddTriggerTransitionFromAny(EnemyStateEvent.OnDying, EnemyState.Dying, forceInstantly: true);
            StateMachine.AddTransition(EnemyState.Dying, EnemyState.Dead);
        }

        #region    EVENT_METHODS
        #endregion =============

        protected virtual void OnEnterAppearState(State<EnemyState, EnemyStateEvent> state)
        {

        }

        protected virtual void OnExitAppearState(State<EnemyState, EnemyStateEvent> state)
        {
            navMeshAgent.enabled = true;
            characterPhysicsComponent.Activate();
        }

        protected virtual void OnEnterNormalAttackState(State<EnemyState, EnemyStateEvent> state) { }

        protected virtual void OnExitNormalAttackState(State<EnemyState, EnemyStateEvent> state) { }

        protected virtual void OnEnterSpecialAttackState(State<EnemyState, EnemyStateEvent> state) { }

        protected virtual void OnExitSpecialAttackState(State<EnemyState, EnemyStateEvent> state) { }

        protected virtual void OnEnterDyingState(State<EnemyState, EnemyStateEvent> state)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.enabled = false;
            CharacterPhysicsComponent.Deactivate();
        }

        protected virtual void OnEnterDeadState(State<EnemyState, EnemyStateEvent> state)
        {
            OnReturnToPool();
        }

        public void TriggerDying()
        {
            isAlive = false;
            StateMachine.Trigger(EnemyStateEvent.OnDying);
        }
    }
}
