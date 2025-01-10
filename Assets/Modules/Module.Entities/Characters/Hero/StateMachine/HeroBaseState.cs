using JetBrains.Annotations;
using Module.Core.HFSM;
using Module.Core.HFSM.States;
using UnityEngine;

namespace Module.Entities.Characters.Hero.StateMachine
{
    public class HeroBaseState : State<HeroState, HeroStateEvent>
    {
        protected readonly MonoHeroController Controller;
        protected readonly CharacterAnimationComponent CharacterAnimationComponent;

        protected readonly StateFunc<HeroState, HeroStateEvent> OnEnterState;
        protected readonly StateFunc<HeroState, HeroStateEvent> OnExitState;

        protected AnimatorStateInfo animatorStateInfo;

        protected float exitTime;
        protected bool requestExit;

        public HeroBaseState(
            [NotNull] MonoHeroController controller
            , bool needsExitTime
            , float exitTime = 0.1F
            , StateFunc<HeroState, HeroStateEvent> onEnterState = default
            , StateFunc<HeroState, HeroStateEvent> onExitState = default)
        {
            OnEnterState = onEnterState;
            OnExitState = onExitState;

            this.needsExitTime = needsExitTime;
            this.exitTime = exitTime;

            Controller = controller;
            CharacterAnimationComponent = controller.CharacterAnimationComponent;
        }

        public override void OnEnter()
        {
            requestExit = false;
            Timer.Reset();
            OnEnterState?.Invoke(this);
        }

        public override void OnLogic()
        {
            if(requestExit && Timer.ElapsedTime >= exitTime)
            {
                StateMachine.StateCanExit();
            }
        }

        public override void OnExitRequest()
        {
            if(needsExitTime == false)
            {
                StateMachine.StateCanExit();
                return;
            }

            requestExit = true;
        }
    }
}
