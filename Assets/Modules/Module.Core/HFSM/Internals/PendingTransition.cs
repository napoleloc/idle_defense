using Module.Core.HFSM.Transitions;

namespace Module.Core.HFSM.Internals
{
    internal struct PendingTransition<TStateId>
        where TStateId : unmanaged
    {
        public ITransitionLifecycleListener lifecycleListener;
        public TStateId stateId;

        public bool isExitTransition;
        public bool isPending;

        public PendingTransition(ITransitionLifecycleListener listener = null)
        {
            isExitTransition = true;
            isPending = true;

            stateId = default(TStateId);
            lifecycleListener = listener;
        }

        public PendingTransition(TStateId target, ITransitionLifecycleListener listener = null)
        {
            isExitTransition = false;
            isPending = true;

            stateId = target;
            lifecycleListener = listener;
        }
    }
}
