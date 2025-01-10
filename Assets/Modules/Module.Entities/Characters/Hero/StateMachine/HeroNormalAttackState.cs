using JetBrains.Annotations;

namespace Module.Entities.Characters.Hero.StateMachine
{
    public class HeroNormalAttackState : HeroBaseState
    {
        public HeroNormalAttackState([NotNull] MonoHeroController controller, bool needsExitTime) : base(controller, needsExitTime)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();

            CharacterAnimationComponent.CrossFadeAnim(GameCommon.Animation.CharAnim.NormalAttack);
        }

        public override void OnLogic()
        {
            if(CharacterAnimationComponent.TryGetAnimatorStateInfo(GameCommon.Animation.CharAnim.NormalAttack, ref animatorStateInfo))
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
