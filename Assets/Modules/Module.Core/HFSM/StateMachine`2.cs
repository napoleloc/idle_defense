namespace Module.Core.HFSM
{
    public class StateMachine<TStateId, TEvent> : StateMachine<TStateId, TStateId, TEvent>
        where TStateId : unmanaged
        where TEvent : unmanaged
    {
    
    }
}
