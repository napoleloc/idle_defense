
namespace Module.Core.HFSM
{
    public interface IStateMachine
    {
        /// <summary>
        /// Tells the state machine that, if there is a state transition pending,
        /// now is the time to perform it.
        /// </summary>
        bool HasPendingTransition { get; }

        void StateCanExit();
    }
}

