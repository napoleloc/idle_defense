using System;

namespace Module.Core.HFSM.Transitions
{
    public class TransitionScheduled<TStateId> : Transition<TStateId>
        where TStateId : unmanaged
    {
        public readonly Predicate<TransitionScheduled<TStateId>> Condition;
        public readonly Action<TransitionScheduled<TStateId>> BeforeTransition;
        public readonly Action<TransitionScheduled<TStateId>> AfterTransition;

        public readonly ITimer Timer;
        public readonly float Delay;

        public TransitionScheduled(
            TStateId from
            , TStateId to
            , float delayTime
            , Predicate<TransitionScheduled<TStateId>> condition
            , Action<TransitionScheduled<TStateId>> beforeTransition
            , Action<TransitionScheduled<TStateId>> afterTransition
            , bool forceInstantly = false
            ) : base(from, to, forceInstantly)
        {
            Condition = condition;
            BeforeTransition = beforeTransition;
            AfterTransition = afterTransition;

            Timer = new Timer();
            Delay = delayTime;
        }

        public override void OnEnter()
        {
            Timer.Reset();
        }

        public override bool ShouldTransition()
        {
            if (Timer.ElapsedTime < Delay)
            {
                return false;
            }

            if (Condition == null)
            {
                return true;
            }

            return Condition(this);
        }

        public override void OnBeforeTransition()
            => BeforeTransition?.Invoke(this);

        public override void OnAfterTransition()
            => AfterTransition?.Invoke(this);
    }
}
