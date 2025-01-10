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
        }
    }
}
