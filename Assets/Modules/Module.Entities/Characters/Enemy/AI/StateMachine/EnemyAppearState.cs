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
            , float exitTime = 0.99f) : base(controller, needsExitTime, exitTime, onEnterState, onExitState
        )
        {

        }

        public override void OnLogic()
        {
            if (CharacterAnimationComponent.TryGetAnimatorStateInfo(GameCommon.Animation.CharAnim.Spawn, ref animatorStateInfo))
            {
                float currentTime = animatorStateInfo.normalizedTime % 1;
                if (currentTime >= 0.99F)
                {
                    StateMachine.StateCanExit();
                }

            }
        }

        public override void OnExit()
        {
            OnExitState?.Invoke(this);
        }
    }
}
