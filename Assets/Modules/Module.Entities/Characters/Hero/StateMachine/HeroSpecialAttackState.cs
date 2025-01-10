using JetBrains.Annotations;

namespace Module.Entities.Characters.Hero.StateMachine
{
    public class HeroSpecialAttackState : HeroBaseState
    {
        public HeroSpecialAttackState([NotNull] MonoHeroController controller, bool needsExitTime) : base(controller, needsExitTime)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
        }
    }
}
