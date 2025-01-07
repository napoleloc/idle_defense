using JetBrains.Annotations;
using Module.Core.HFSM;
using Module.Core.HFSM.States;
using UnityEngine;
using UnityEngine.AI;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyBaseState : State<EnemyState, EnemyStateEvent>
    {
        protected readonly EnemyAIController Controller;
        protected readonly NavMeshAgent NavMeshAgent;

        protected readonly StateFunc<EnemyState, EnemyStateEvent> OnEnterState;
        protected readonly StateFunc<EnemyState, EnemyStateEvent> OnExitState;

        protected AnimatorStateInfo animatorStateInfo;

        protected float exitTime;
        protected bool requestExit;

        public EnemyBaseState(
            [NotNull] EnemyAIController controller
            , bool needsExitTime
            , float exitTime = 0.1F
            , StateFunc<EnemyState, EnemyStateEvent> onEnterState = default
            , StateFunc<EnemyState, EnemyStateEvent> onExitState = default)
        {
            OnEnterState = onEnterState;
            OnExitState = onExitState;

            this.needsExitTime = needsExitTime;
            this.exitTime = exitTime;

            Controller = controller;
            NavMeshAgent = controller.NavMeshAgent;
        }

        public override void OnEnter()
        {
            requestExit = false;
            Timer.Reset();
            OnEnterState?.Invoke(this);
        }

        public override void OnLogic()
        {
            if(requestExit && Timer.ElapsedTime >= exitTime)
            {
                StateMachine.StateCanExit();
            }
        }

        public override void OnExitRequest()
        {
            if(needsExitTime == false)
            {
                StateMachine.StateCanExit();
                return;
            }

            requestExit = true;
        }
    }
}
