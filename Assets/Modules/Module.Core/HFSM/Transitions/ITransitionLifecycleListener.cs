namespace Module.Core.HFSM.Transitions
{
    public interface ITransitionLifecycleListener
    {
        void BeforeTransition();
        void AfterTransition();
    }
}
