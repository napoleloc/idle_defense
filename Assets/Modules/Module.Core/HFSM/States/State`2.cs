namespace Module.Core.HFSM.States
{
    public class State<TStateId, TEvent> : State<TStateId>
        where TStateId : unmanaged
        where TEvent : unmanaged
    {
        public readonly ITimer Timer;

        public State(bool needsExitTime = false, bool isGhostState = false)
            : base(needsExitTime, isGhostState)
        {
            Timer = new Timer();
        }
    }
}
