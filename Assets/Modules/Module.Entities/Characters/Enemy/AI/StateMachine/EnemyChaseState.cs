using JetBrains.Annotations;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyChaseState : EnemyBaseState
    {
        public EnemyChaseState([NotNull] EnemyAIController controller, bool needsExitTime) : base(controller, needsExitTime)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
