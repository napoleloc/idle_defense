using Module.Core.HFSM.States;

namespace Module.Core.HFSM
{
    public delegate void StateFuncIn<TStateId, TEvent>();
    public delegate void StateFuncRef<TStateId, TEvent>();
    public delegate void StateFunc<TStateId, TEvent>(State<TStateId, TEvent> state)
        where TStateId : unmanaged
        where TEvent : unmanaged;
}
