using JetBrains.Annotations;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(
            [NotNull] EnemyAIController controller
            , bool needsExitTime
            , float exitTime = 0.5F) : base(controller, needsExitTime, exitTime
        )
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();

            CharacterAnimationComponent.CrossFadeAnim(GameCommon.Animation.CharAnim.Idle);
        }
    }
}
