using System.Runtime.CompilerServices;

namespace Module.Core.HFSM.States
{
    public class State<TStateId> where TStateId : unmanaged
    {
        public TStateId name;
        public bool needsExitTime;
        public bool isGhostState;

        public IStateMachine StateMachine { get; set; }

        public State(bool needsExitTime = false, bool isGhostState = false)
        {
            this.needsExitTime = needsExitTime;
            this.isGhostState = isGhostState;
        }


        /// <summary>
		/// Called to initialise the state, after values like name and fsm have been set.
		/// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Called when the state machine transitions to this state (enters this state).
        /// </summary>
        public virtual void OnEnter()
        {

        }

        /// <summary>
		/// Called while this state is active.
		/// </summary>
		public virtual void OnLogic()
        {

        }

        /// <summary>
        /// Called when the state machine transitions from this state to another state (exits this state).
        /// </summary>
        public virtual void OnExit()
        {

        }

        /// <summary>
		/// (Only if needsExitTime is true):
		/// 	Called when a state transition from this state to another state should happen.
		/// 	If it can exit, it should call fsm.StateCanExit()
		/// 	and if it can not exit right now, it should call fsm.StateCanExit() later in e.g. OnLogic().
		/// </summary>
		public virtual void OnExitRequest()
        {

        }

        /// <summary>
        /// Returns a string representation of all active states in the hierarchy,
        /// e.g. "/Move/Jump/Falling".
        /// In contrast, the state machine's ActiveStateName property only returns the name
        /// of its active state, not of any nested states.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual string GetToString()
        {
            return name.ToString();
        }
    }
}
