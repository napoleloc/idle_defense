using EncosyTower.Modules;
using Module.Core.Extended.PubSub;
using Module.Core.HFSM;
using Module.Core.HFSM.States;
using Module.Entities.Characters.Enemy.PubSub;
using UnityEngine;
using UnityEngine.AI;

namespace Module.Entities.Characters.Enemy.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAIController : MonoBehaviour, IEntityComponent, IEntityPoolable
    {
        protected readonly StateMachine<EnemyState, EnemyStateEvent> StateMachine;

        protected NavMeshAgent navMeshAgent;
        protected CharacterController characterController;
        protected CharacterPhysicsComponent characterPhysicsComponent;

        protected Id uniqueId;
        protected bool isAlive;

        public NavMeshAgent NavMeshAgent => navMeshAgent;
        public CharacterController CharacterController => characterController;
        public CharacterPhysicsComponent CharacterPhysicsComponent => characterPhysicsComponent;

        public bool IsAlive => isAlive;

        private void Awake()
        {
            InitializeDependencies();

            uniqueId = new(gameObject.GetInstanceID());
        }

        private void Update()
        {
            if (isAlive)
            {
                UpdateComponents();
            }
        }

        private void UpdateComponents()
        {
            characterPhysicsComponent.ApplyGravity();
            StateMachine.OnLogic();
        }

        public void InitializeDependencies()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            characterPhysicsComponent = GetComponent<CharacterPhysicsComponent>();

            navMeshAgent.enabled = false;
            characterPhysicsComponent.Deactivate();
        }

        public void OnGetFromPool()
        {
            WorldMessenger.Publisher.EnemyScope()
                .Publish(new RegisterEnemyMessage(uniqueId, gameObject)); 
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
            StateMachine.AddState(EnemyState.Appear, new EnemyAppearState(this, true, OnEnterAppearState, OnExitAppearState));
            StateMachine.AddState(EnemyState.Idle, new EnemyIdleState(this, false));
            StateMachine.AddState(EnemyState.Chase, new EnemyChaseState(this, false));
            StateMachine.AddState(EnemyState.Dying, new EnemyDyingState(this, true, OnEnterDyingState));
            StateMachine.AddState(EnemyState.Dead, new EnemyDeadState(this, true));

            InitTransitionConditions();

            StateMachine.Initialize();
        }

        protected virtual void InitTransitionConditions()
        {

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

        protected virtual void OnEnterDyingState(State<EnemyState, EnemyStateEvent> state)
        {

        }
    }
}
