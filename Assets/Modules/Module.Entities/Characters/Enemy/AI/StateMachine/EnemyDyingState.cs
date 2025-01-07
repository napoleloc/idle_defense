using JetBrains.Annotations;
using Module.Core.HFSM;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyDyingState : EnemyBaseState
    {
        public EnemyDyingState(
            [NotNull] EnemyAIController controller
            , bool needsExitTime
            , StateFunc<EnemyState, EnemyStateEvent> onEnterState
            , float exitTime = 0.33F) : base(controller, needsExitTime, exitTime, onEnterState
        )
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
