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

            CharacterAnimationComponent.CrossFadeAnim(GameCommon.Animation.CharAnim.Dying);
        }

        public override void OnLogic()
        {
            if (CharacterAnimationComponent.TryGetAnimatorStateInfo(GameCommon.Animation.CharAnim.Dying, ref animatorStateInfo))
            {
                float currentTime = animatorStateInfo.normalizedTime % 1;
                if (currentTime >= 0.99F)
                {
                    StateMachine.StateCanExit();
                }
            }
        }
    }
}
