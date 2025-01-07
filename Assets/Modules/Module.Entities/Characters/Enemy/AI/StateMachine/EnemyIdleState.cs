using JetBrains.Annotations;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState([NotNull] EnemyAIController controller, bool needsExitTime) : base(controller, needsExitTime)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
