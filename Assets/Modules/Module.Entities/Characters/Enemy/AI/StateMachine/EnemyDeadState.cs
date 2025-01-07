using JetBrains.Annotations;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyDeadState : EnemyBaseState
    {
        public EnemyDeadState([NotNull] EnemyAIController controller, bool needsExitTime) : base(controller, needsExitTime) { }

        public override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
