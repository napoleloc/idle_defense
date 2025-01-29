using System.Diagnostics.CodeAnalysis;
using Module.Core.HFSM;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemySpecialAttackState : EnemyBaseState
    {
        public EnemySpecialAttackState([NotNull] EnemyAIController controller
            , bool needsExitTime
            , StateFunc<EnemyState, EnemyStateEvent> onEnterState
            , StateFunc<EnemyState, EnemyStateEvent> onExitState
            , float exitTime = 0.33F) : base(controller, needsExitTime , exitTime, onEnterState, onExitState)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            OnExitState?.Invoke(this);
        }
    }
}
