using JetBrains.Annotations;
using Module.Core.HFSM;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyAppearState : EnemyBaseState
    {
        public EnemyAppearState(
            [NotNull] EnemyAIController controller
            , bool needsExitTime
            , StateFunc<EnemyState, EnemyStateEvent> onEnterState
            , StateFunc<EnemyState, EnemyStateEvent> onExitState
            , float exitTime = 0.33f) : base(controller, needsExitTime, exitTime, onEnterState, onExitState
        )
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
