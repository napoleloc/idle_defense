namespace Module.Core.HFSM.Transitions
{
    public class Transition<TStateId>
        where TStateId : unmanaged
    {
        public IStateMachine stateMachine;

        public TStateId from;
        public TStateId to;

        public bool forceInstantly;
        public bool isExitTransition;

        public Transition(TStateId from, TStateId to, bool forceInstantly = false)
        {
            this.from = from;
            this.to = to;
            this.forceInstantly = forceInstantly;
            this.isExitTransition = false;
        }

        /// <summary>
		/// Called to initialize the transition, after values like fsm have been set.
		/// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
		/// Called when the state machine enters the "from" state.
		/// </summary>
		public virtual void OnEnter()
        {

        }

        /// <summary>
        /// Called to determine whether the state machine should transition to the <c>to</c> state.
        /// </summary>
        /// <returns>True if the state machine should change states / transition.</returns>
        public virtual bool ShouldTransition()
        {
            return true;
        }

        /// <summary>
        /// Callback method that is called just before the transition happens.
        /// </summary>
        public virtual void OnBeforeTransition()
        {

        }

        /// <summary>
        /// Callback method that is called just after the transition happens.
        /// </summary>
        public virtual void OnAfterTransition()
        {

        }
    }
}
