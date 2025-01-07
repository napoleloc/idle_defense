using System;

namespace Module.Core.HFSM.Transitions
{
    public class TransitionCondition<TStateId> : Transition<TStateId>
        where TStateId : unmanaged
    {
        public readonly Predicate<TransitionCondition<TStateId>> Condition;
        public readonly Action<TransitionCondition<TStateId>> BeforeTransition;
        public readonly Action<TransitionCondition<TStateId>> AfterTransition;

        public TransitionCondition(
            TStateId from
            , TStateId to
            , Predicate<TransitionCondition<TStateId>> condition = null
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
            ) : base(from, to, forceInstantly)
        {
            Condition = condition;
            BeforeTransition = beforeTransition;
            AfterTransition = afterTransition;
        }

        public override bool ShouldTransition()
        {
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
