using EncosyTower.Modules;

namespace Module.Core.HFSM.Transitions
{
    public class TransitionWrapper<TStateId>
           where TStateId : unmanaged
    {
        private readonly ActionIn<Transition<TStateId>> _beforeOnEnter;
        private readonly ActionIn<Transition<TStateId>> _afterOnEnter;

        private readonly ActionIn<Transition<TStateId>> _beforeShouldTransition;
        private readonly ActionIn<Transition<TStateId>> _afterShouldTransition;

        public TransitionWrapper(
            ActionIn<Transition<TStateId>> beforeOnEnter = null
            , ActionIn<Transition<TStateId>> afterOnEnter = null
            , ActionIn<Transition<TStateId>> beforeShouldTransition = null
            , ActionIn<Transition<TStateId>> afterShouldTransition = null)
        {
            _beforeOnEnter = beforeOnEnter;
            _afterOnEnter = afterOnEnter;

            _beforeShouldTransition = beforeShouldTransition;
            _afterShouldTransition = afterShouldTransition;
        }

        public WrappedTransition Wrap(Transition<TStateId> transition)
        {
            return new WrappedTransition(
                transition
                , _beforeOnEnter
                , _afterOnEnter
                , _beforeShouldTransition
                , _afterShouldTransition);
        }

        public class WrappedTransition : Transition<TStateId>
        {
            private readonly ActionIn<Transition<TStateId>> _beforeOnEnter;
            private readonly ActionIn<Transition<TStateId>> _afterOnEnter;

            private readonly ActionIn<Transition<TStateId>> _beforeShouldTransition;
            private readonly ActionIn<Transition<TStateId>> _afterShouldTransition;

            private Transition<TStateId> _transition;

            public WrappedTransition(
                Transition<TStateId> transition
                , ActionIn<Transition<TStateId>> beforeOnEnter = null
                , ActionIn<Transition<TStateId>> afterOnEnter = null
                , ActionIn<Transition<TStateId>> beforeShouldTransition = null
                , ActionIn<Transition<TStateId>> afterShouldTransition = null
                ) : base(transition.from, transition.to, transition.forceInstantly)
            {
                _transition = transition;

                _beforeOnEnter = beforeOnEnter;
                _afterOnEnter = afterOnEnter;

                _beforeShouldTransition = beforeShouldTransition;
                _afterShouldTransition = afterShouldTransition;
            }

            public override void Initialize()
            {
                _transition.stateMachine = this.stateMachine;
            }

            public override void OnEnter()
            {
                _beforeOnEnter?.Invoke(_transition);
                _transition.OnEnter();
                _afterOnEnter?.Invoke(_transition);
            }

            public override bool ShouldTransition()
            {
                _beforeShouldTransition?.Invoke(_transition);
                bool shouldTransition = _transition.ShouldTransition();
                _afterShouldTransition?.Invoke(_transition);
                return shouldTransition;
            }

            public override void OnBeforeTransition()
            {
                _transition.OnBeforeTransition();
            }

            public override void OnAfterTransition()
            {
                _transition.OnAfterTransition();
            }
        }
    }
}
