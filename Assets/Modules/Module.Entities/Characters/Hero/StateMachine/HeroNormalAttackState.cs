using Module.GameCommon.Animation;
using JetBrains.Annotations;

namespace Module.Entities.Characters.Hero.StateMachine
{
    public class HeroNormalAttackState : HeroBaseState
    {
        private readonly MonoHeroTargetFindingComponent _targetFindingComponent;
        private bool _isAttacking = false;

        public HeroNormalAttackState([NotNull] MonoHeroController controller, bool needsExitTime) : base(controller, needsExitTime)
        {
            _targetFindingComponent = controller.TargetFindingComponent;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CharacterAnimationComponent.CrossFadeAnim(CharAnim.NormalAttack);
        }

        public override void OnLogic()
        {
            if(CharacterAnimationComponent.TryGetAnimatorStateInfo(CharAnim.NormalAttack, ref animatorStateInfo))
            {
                float currentTime = animatorStateInfo.normalizedTime % 1;

                if(_isAttacking == false  
                    && CharacterAnimationComponent.CanTriggerEvent(CharAnim.NormalAttack, currentTime))
                {
                    _targetFindingComponent.TargetEnemy.TriggerDying();
                    _isAttacking = true;
                }

                if (currentTime >= 0.99F)
                {
                    StateMachine.StateCanExit();
                }
            }
        }

        public override void OnExit()
        {
            _isAttacking = false;
        }
    }
}
