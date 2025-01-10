using JetBrains.Annotations;

namespace Module.Entities.Characters.Hero.StateMachine
{
    public class HeroIdleState : HeroBaseState
    {
        public HeroIdleState([NotNull] MonoHeroController controller, bool needsExitTime) : base(controller, needsExitTime)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();

            CharacterAnimationComponent.CrossFadeAnim(GameCommon.Animation.CharAnim.Idle);
        }
    }
}
