using System.Runtime.CompilerServices;
using System;

namespace Module.Core.HFSM.States
{
    public class StateWrapper<TStateId, TEvent>
        where TStateId : unmanaged
        where TEvent : unmanaged
    {
        private readonly Action<State<TStateId>> _beforeOnEnter;
        private readonly Action<State<TStateId>> _afterOnEnter;

        private readonly Action<State<TStateId>> _beforeOnLogic;
        private readonly Action<State<TStateId>> _afterOnLogic;

        private readonly Action<State<TStateId>> _beforeOnExit;
        private readonly Action<State<TStateId>> _afterOnExit;

        public StateWrapper(
            Action<State<TStateId>> beforeOnEnter = null
            , Action<State<TStateId>> afterOnEnter = null
            , Action<State<TStateId>> beforeOnLgic = null
            , Action<State<TStateId>> afterOnLgic = null
            , Action<State<TStateId>> beforeOnExit = null
            , Action<State<TStateId>> afterOnExit = null
        )
        {
            _beforeOnEnter = beforeOnEnter;
            _afterOnEnter = afterOnEnter;

            _beforeOnLogic = beforeOnLgic;
            _afterOnLogic = afterOnLgic;

            _beforeOnExit = beforeOnExit;
            _afterOnExit = afterOnExit;
        }

        public WrappedState Wrap(State<TStateId> state)
        {
            return new WrappedState(state
                , _beforeOnEnter
                , _afterOnEnter
                , _beforeOnLogic
                , _afterOnLogic
                , _beforeOnExit
                , _afterOnExit);
        }

        #region    WRAPPED_STATE
        #endregion =============

        public class WrappedState : State<TStateId>, IActionable<TEvent>, ITriggerable<TEvent>
        {
            private readonly Action<State<TStateId>> _beforeOnEnterState;
            private readonly Action<State<TStateId>> _afterOnEnterState;

            private readonly Action<State<TStateId>> _beforeOnLogicState;
            private readonly Action<State<TStateId>> _afterOnLogicState;

            private readonly Action<State<TStateId>> _beforeOnExitState;
            private readonly Action<State<TStateId>> _afterOnExitState;

            private readonly State<TStateId> _targetState;

            internal WrappedState(
                State<TStateId> targetState
                , Action<State<TStateId>> beforeOnEnter = null
                , Action<State<TStateId>> afterOnEnter = null
                , Action<State<TStateId>> beforeOnLgic = null
                , Action<State<TStateId>> afterOnLgic = null
                , Action<State<TStateId>> beforeOnExit = null
                , Action<State<TStateId>> afterOnExit = null
            )
            {
                _targetState = targetState;

                _beforeOnEnterState = beforeOnEnter;
                _afterOnEnterState = afterOnEnter;

                _beforeOnLogicState = beforeOnLgic;
                _afterOnLogicState = afterOnLgic;

                _beforeOnExitState = beforeOnExit;
                _afterOnExitState = afterOnExit;
            }

            public override void Initialize()
            {
                _targetState.name = name;

                _targetState.Initialize();
            }

            public override void OnEnter()
            {
                _beforeOnEnterState?.Invoke(this);
                _targetState.OnEnter();
                _afterOnEnterState?.Invoke(this);
            }

            public override void OnLogic()
            {
                _beforeOnLogicState?.Invoke(this);
                _targetState.OnLogic();
                _afterOnLogicState?.Invoke(this);
            }

            public override void OnExit()
            {
                _beforeOnExitState?.Invoke(this);
                _targetState.OnExit();
                _afterOnExitState?.Invoke(this);
            }

            public override void OnExitRequest()
            {
                _targetState.OnExitRequest();
            }

            public void Trigger(TEvent trigger)
            {
                (_targetState as ITriggerable<TEvent>)?.Trigger(trigger);
            }

            public void OnAction(TEvent trigger)
            {
                (_targetState as IActionable<TEvent>)?.OnAction(trigger);
            }

            public void OnAction<TData>(TEvent trigger, TData data)
            {
                (_targetState as IActionable<TEvent>)?.OnAction<TData>(trigger, data);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override string GetToString()
            {
                return _targetState.GetToString();
            }
        }
    }
}
