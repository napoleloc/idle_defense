namespace Module.Core.HFSM.Transitions
{
    /// <summary>
    /// A ReverseTransition wraps another transition, but reverses it. The "from"
    /// and "to" states are swapped. Only when the condition of the wrapped transition
    /// is false does it transition.
    /// The BeforeTransition and AfterTransition callbacks of the the wrapped transition
    /// are also swapped.
    /// </summary>
    public class ReverseTransition<TStateId> : Transition<TStateId>
        where TStateId : unmanaged
    {
        private bool _shouldInitWrappedTransition;

        public Transition<TStateId> WrappedTransition { get; init; }

        public ReverseTransition(
            Transition<TStateId> wrappedTransition
            , bool shouldInitWrappedTransition = true
            ) : base(wrappedTransition.to, wrappedTransition.from, wrappedTransition.forceInstantly)

        {
            WrappedTransition = wrappedTransition;
            _shouldInitWrappedTransition = shouldInitWrappedTransition;
        }

        public override void Initialize()
        {
            if (_shouldInitWrappedTransition)
            {
                WrappedTransition.stateMachine = this.stateMachine;
                WrappedTransition.Initialize();
            }
        }

        public override void OnEnter()
        {
            WrappedTransition.OnEnter();
        }

        public override bool ShouldTransition()
        {
            return WrappedTransition.ShouldTransition() == false;
        }

        public override void OnBeforeTransition()
        {
            WrappedTransition.OnAfterTransition();
        }

        public override void OnAfterTransition()
        {
            WrappedTransition.OnBeforeTransition();
        }
    }
}
