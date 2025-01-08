using JetBrains.Annotations;
using Module.Core.HFSM;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyDeadState : EnemyBaseState
    {
        public EnemyDeadState(
            [NotNull] EnemyAIController controller
            , bool needsExitTime
            , StateFunc<EnemyState, EnemyStateEvent> onEnterState
            , float exitTime) : base(controller, needsExitTime, exitTime, onEnterState
        )
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
