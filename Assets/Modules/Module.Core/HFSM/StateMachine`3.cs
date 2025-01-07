using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Logging;
using Module.Core.HFSM.Internals;
using Module.Core.HFSM.States;
using Module.Core.HFSM.Transitions;
using UnityEditorInternal;

namespace Module.Core.HFSM
{
    public class StateMachine<TOwnId, TStateId, TEvent> : State<TOwnId>
        , ITriggerable<TEvent>, IActionable<TEvent>
        , IStateMachine, IDisposable
        where TOwnId : unmanaged
        where TStateId : unmanaged
        where TEvent : unmanaged
    {
        private readonly FasterList<Transition<TStateId>> _transitionFromAny;

        private readonly ArrayMap<TStateId, StateBundle<TStateId, TEvent>> _idToBundleMap;
        private readonly ArrayMap<TEvent, FasterList<Transition<TStateId>>> _triggerTransitionsFromAny;

        private State<TStateId> _activeState;

        private PendingTransition<TStateId> _pendingTransition;
        private TStateId _startState;

        private bool _hasState;
        private bool _rememberLastState;

        private event Action<State<TStateId>> StateChanged;

        public StateMachine(bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
            : base(needsExitTime, isGhostState)
        {
            _rememberLastState = rememberLastState;

            _transitionFromAny = new();
            _idToBundleMap = new();
            _triggerTransitionsFromAny = new();
        }

        public State<TStateId> ActiveState
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _activeState;
        }

        public TStateId ActiveStateName
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _activeState.name;
        }

        public bool HasPendingTransition
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => StateMachine.HasPendingTransition;
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => StateMachine != null;
        }

        #region    OVERRIDE_METHODS
        #endregion ================

        public override void Initialize()
        {
            if (IsValid == false)
            {
                return;
            }

            OnEnter();
        }

        public override void OnEnter()
        {
            if (_hasState == false)
            {
                return;
            }

            _pendingTransition = default;

            ChangeState(_startState);

            var transitionsFromAny = _transitionFromAny.AsReadOnlySpan();
            var length = transitionsFromAny.Length;

            if (length >= 1)
            {
                for (var i = 0; i < length; i++)
                {
                    transitionsFromAny[i].OnEnter();
                }
            }

            lock (_transitionFromAny)
            {
                var span = _triggerTransitionsFromAny.GetValues();
                var lenght = span.Length;

                if (lenght >= 1)
                {
                    for (var i = 0; i < lenght; i++)
                    {
                        var transitions = span[i].AsReadOnlySpan();
                        var loopCount = transitions.Length;

                        for (var counter = 0; counter < loopCount; counter++)
                        {
                            transitions[counter].OnEnter();
                        }
                    }
                }
            }
        }

        public override void OnLogic()
        {
            if (TryAllGlobalTransitions())
                goto runOnLogic;

            if (TryAllDirectTransitions())
                goto runOnLogic;

            runOnLogic:
            _activeState?.OnLogic();
        }

        public override void OnExit()
        {
            if (_activeState == null)
            {
                return;
            }

            if (_rememberLastState)
            {
                _startState = _activeState.name;
                _hasState = true;
            }

            _activeState.OnExit();
            _activeState = default;
        }

        public override void OnExitRequest()
        {
            if (_activeState.needsExitTime)
            {
                _activeState.OnExitRequest();
            }
        }

        /// <summary>
		/// Runs an action on the currently active state.
		/// </summary>
		/// <param name="trigger">Name of the action.</param>
		public virtual void OnAction(TEvent trigger)
        {
            //EnsureIsInitializedFor("Running OnAction of the active state");
            (_activeState as IActionable<TEvent>)?.OnAction(trigger);
        }

        /// <summary>
        /// Runs an action on the currently active state and lets you pass one data parameter.
        /// </summary>
        /// <param name="trigger">Name of the action.</param>
        /// <param name="data">Any custom data for the parameter.</param>
        /// <typeparam name="TData">Type of the data parameter.
        /// 	Should match the data type of the action that was added via AddAction<T>(...).</typeparam>
        public virtual void OnAction<TData>(TEvent trigger, TData data)
        {
            //EnsureIsInitializedFor("Running OnAction of the active state");
            (_activeState as IActionable<TEvent>)?.OnAction<TData>(trigger, data);
        }

        #region    STATE<TStateId>
        #endregion ===============

        public void AddState(TStateId id, State<TStateId> state)
        {
            state.StateMachine = this;
            state.name = id;
            state.Initialize();

            var bundle = GetOrCreateStateBundle(id);
            bundle.State = state;

            if (_idToBundleMap.Count == 1 && _hasState == false)
            {
                SetStartState(id);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetStartState(TStateId id)
        {
            _startState = id;
            _hasState = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(TStateId id, out State<TStateId> result)
        {
            if (_idToBundleMap.TryGetValue(id, out var weakRef) == true)
            {
                result = weakRef.State;
                return true;
            }

            result = default;
            return false;
        }

        public void RequestStateChange(
            TStateId id
            , bool forceInstantly = false
            , ITransitionLifecycleListener listener = default
        )
        {
            if (_activeState.needsExitTime == false || forceInstantly)
            {
                _pendingTransition = default;
                ChangeState(id, listener);
            }
            else
            {
                _pendingTransition = new(id, listener);
                _activeState.OnExitRequest();
            }
        }

        public void RequestExit(bool forceInstantly = false, ITransitionLifecycleListener listener = default)
        {
            if (_activeState.needsExitTime || forceInstantly)
            {
                _pendingTransition = default;
                listener?.AfterTransition();
                PerformVerticalTransition();
                listener.AfterTransition();
            }
            else
            {
                _pendingTransition = new(listener);
                _activeState.OnExitRequest();
            }
        }

        private void ChangeState(TStateId id, ITransitionLifecycleListener listener = default)
        {
            listener?.BeforeTransition();
            _activeState?.OnExit();

            if (_idToBundleMap.TryGetValue(id, out var bundle) == false || bundle.State == null)
            {
                DevLoggerAPI.LogError($"The state associated with id '{id}' is null.");
                return;
            }

            _activeState = bundle.State;
            _activeState.OnEnter();

            var transitions = bundle.Transitions.Span;
            var length = transitions.Length;

            if (length >= 1)
            {
                for (int i = 0; i < length; i++)
                {
                    transitions[i].OnEnter();
                }
            }

            var triggerTransitions = bundle.EventToTransitionMap;
            foreach (var (key, value) in triggerTransitions)
            {
                var span = value.AsSpan();
                var loopCount = span.Length;

                if (loopCount >= 1)
                {
                    for (int i = 0; i < loopCount; i++)
                    {
                        span[i].OnEnter();
                    }
                }
            }

            listener?.AfterTransition();

            OnStateChanged();

            if (_activeState.isGhostState)
            {
                TryAllDirectTransitions();
            }

        }

        public void StateCanExit()
        {
            if (_pendingTransition.isPending == false)
            {
                return;
            }

            var listener = _pendingTransition.lifecycleListener;
            if (_pendingTransition.isExitTransition)
            {
                _pendingTransition = default;

                listener?.BeforeTransition();
                PerformVerticalTransition();
                listener?.AfterTransition();
            }
            else
            {
                var state = _pendingTransition.stateId;

                _pendingTransition = default;
                ChangeState(state, listener);
            }
        }

        private void OnStateChanged()
           => StateChanged?.Invoke(_activeState);

        #region    TRANSITION<TStateId>
        #endregion ====================

        /// <summary>
        /// Adds a new transition between two states.
        /// </summary>
        /// <param name="transition">The transition instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddTransition(Transition<TStateId> transition)
        {
            InitializeTransition(transition);

            var bundle = GetOrCreateStateBundle(transition.from);
            bundle.Add(transition);
        }

        /// <summary>
        /// Adds a new transition that can happen from any possible state.
        /// </summary>
        /// <param name="transition">The transition instance; The "from" field can be
        /// 	left empty, as it has no meaning in this context.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddTransitionFromAny(Transition<TStateId> transition)
        {
            InitializeTransition(transition);

            _transitionFromAny.Add(transition);
        }

        /// <summary>
		/// Adds a new trigger transition between two states that is only checked
		/// when the specified trigger is activated.
		/// </summary>
		/// <param name="trigger">The name / identifier of the trigger.</param>
		/// <param name="transition">The transition instance, e.g. Transition, TransitionAfter, ...</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddTriggerTransition(TEvent trigger, Transition<TStateId> transition)
        {
            InitializeTransition(transition);

            var bundle = GetOrCreateStateBundle(transition.from);
            bundle.Add(trigger, transition);
        }

        /// <summary>
        /// Adds a new trigger transition that can happen from any possible state, but is only
        /// checked when the specified trigger is activated.
        /// </summary>
        /// <param name="trigger">The name / identifier of the trigger</param>
        /// <param name="transition">The transition instance; The "from" field can be
        /// 	left empty, as it has no meaning in this context.</param>
        public void AddTriggerTransitionFromAny(TEvent trigger, Transition<TStateId> transition)
        {
            InitializeTransition(transition);

            lock (_triggerTransitionsFromAny)
            {
                var triggerTransitionsFromAny = _triggerTransitionsFromAny;

                if (triggerTransitionsFromAny.TryGetValue(trigger, out var weakRef) == false)
                {
                    weakRef = new();
                    triggerTransitionsFromAny[trigger] = weakRef;
                }

                weakRef.Add(transition);
            }

        }

        /// <summary>
		/// Adds two transitions:
		/// If the condition of the transition instance is true, it transitions from the "from"
		/// state to the "to" state. Otherwise it performs a transition in the opposite direction,
		/// i.e. from "to" to "from".
		/// </summary>
		/// <remarks>
		/// Internally the same transition instance will be used for both transitions
		/// by wrapping it in a ReverseTransition.
		/// For the reverse transition the afterTransition callback is called before the transition
		/// and the onTransition callback afterwards. If this is not desired then replicate the behaviour
		/// of the two way transitions by creating two separate transitions.
		/// </remarks>
        public void AddTwoWayTransition(Transition<TStateId> transition)
        {
            InitializeTransition(transition);
            AddTransition(transition);

            ReverseTransition<TStateId> reverse = new ReverseTransition<TStateId>(transition, false);
            InitializeTransition(reverse);
            AddTransition(reverse);
        }

        /// <summary>
		/// Adds two transitions that are only checked when the specified trigger is activated:
		/// If the condition of the transition instance is true, it transitions from the "from"
		/// state to the "to" state. Otherwise it performs a transition in the opposite direction,
		/// i.e. from "to" to "from".
		/// </summary>
		/// <remarks>
		/// Internally the same transition instance will be used for both transitions
		/// by wrapping it in a ReverseTransition.
		/// For the reverse transition the afterTransition callback is called before the transition
		/// and the onTransition callback afterwards. If this is not desired then replicate the behaviour
		/// of the two way transitions by creating two separate transitions.
		/// </remarks>
        public void AddTwoWayTriggerTransition(TEvent trigger, Transition<TStateId> transition)
        {
            InitializeTransition(transition);
            AddTriggerTransition(trigger, transition);

            ReverseTransition<TStateId> reverse = new ReverseTransition<TStateId>(transition, false);
            InitializeTransition(reverse);
            AddTriggerTransition(trigger, reverse);
        }

        /// <summary>
		/// Adds a new exit transition from a state. It represents an exit point that
		/// allows the fsm to exit and the parent fsm to continue to the next state.
		/// It is only checked if the parent fsm has a pending transition.
		/// </summary>
		/// <param name="transition">The transition instance. The "to" field can be
		/// 	left empty, as it has no meaning in this context.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddExitTransition(Transition<TStateId> transition)
        {
            transition.isExitTransition = true;
            AddTransition(transition);
        }

        /// <summary>
		/// Adds a new exit transition that can happen from any possible state.
		/// It represents an exit point that allows the fsm to exit and the parent fsm to continue
		/// to the next state. It is only checked if the parent fsm has a pending transition.
		/// </summary>
		/// <param name="transition">The transition instance. The "from" and "to" fields can be
		/// 	left empty, as they have no meaning in this context.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddExitTransitionFromAny(Transition<TStateId> transition)
        {
            transition.isExitTransition = false;
            AddExitTransition(transition);
        }

        /// <summary>
        /// Adds a new exit transition from a state that is only checked when the specified trigger
        /// is activated.
        /// It represents an exit point that allows the fsm to exit and the parent fsm to continue
        /// to the next state. It is only checked if the parent fsm has a pending transition.
        /// </summary>
        /// <param name="transition">The transition instance. The "to" field can be
        /// 	left empty, as it has no meaning in this context.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddExitTriggerTransition(TEvent trigger, Transition<TStateId> transition)
        {
            transition.isExitTransition = true;
            AddTriggerTransition(trigger, transition);
        }

        /// <summary>
        /// Adds a new exit transition that can happen from any possible state and is only checked
        /// when the specified trigger is activated.
        /// It represents an exit point that allows the fsm to exit and the parent fsm to continue
        /// to the next state. It is only checked if the parent fsm has a pending transition.
        /// </summary>
        /// <param name="transition">The transition instance. The "from" and "to" fields can be
        /// 	left empty, as they have no meaning in this context.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddExitTriggerTransitionFromAny(TEvent trigger, Transition<TStateId> transition)
        {
            transition.isExitTransition = true;
            AddTriggerTransitionFromAny(trigger, transition);
        }

        /// <summary>
        /// Activates the specified trigger in all active states of the hierarchy, checking all targeted
        /// trigger transitions to see whether a transition should occur.
        /// </summary>
        /// <param name="trigger">The name / identifier of the trigger.</param>
        public void Trigger(TEvent trigger)
        {
            if (TryTrigger(trigger))
            {
                return;
            }

             (_activeState as ITriggerable<TEvent>)?.Trigger(trigger);
        }

        /// <summary>
        /// Only activates the specified trigger locally in this state machine.
        /// </summary>
        /// <param name="trigger">The name / identifier of the trigger.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TriggerLocally(TEvent trigger)
        {
            TryTrigger(trigger);
        }

        private bool TryTrigger(TEvent trigger)
        {
            lock (_triggerTransitionsFromAny)
            {
                if (_triggerTransitionsFromAny.TryGetValue(trigger, out var weakRefFromAny))
                {
                    var transitions = weakRefFromAny.AsReadOnlySpan();
                    var length = transitions.Length;
                    for (var i = 0; i < length; i++)
                    {
                        var transition = transitions[i];

                        if (EqualityComparer<TStateId>.Default.Equals(transition.to, _activeState.name))
                        {
                            continue;
                        }

                        if (TryTransition(transition))
                        {
                            return true;
                        }
                    }
                }
            }

            lock (_idToBundleMap)
            {
                var id = _activeState.name;

                if (_idToBundleMap.TryGetValue(id, out var bunble))
                {
                    if (bunble.TryGet(trigger, out var span))
                    {
                        var length = span.Length;
                        for (var i = 0; i < length; i++)
                        {
                            var transition = span[i];

                            if (TryTransition(transition))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitializeTransition(Transition<TStateId> transition)
        {
            transition.stateMachine = this;
            transition.Initialize();
        }

        private bool TryTransition(Transition<TStateId> transition)
        {
            if (transition.isExitTransition)
            {
                if (IsValid || StateMachine.HasPendingTransition == false || transition.ShouldTransition() == false)
                {
                    return false;
                }

                RequestExit(transition.forceInstantly, transition as ITransitionLifecycleListener);
                return true;
            }
            else
            {
                if (transition.ShouldTransition() == false)
                {
                    return false;
                }

                RequestStateChange(transition.to, transition.forceInstantly, transition as ITransitionLifecycleListener);
                return true;
            }
        }

        private bool TryAllGlobalTransitions()
        {
            var transitionFromAny = _transitionFromAny.AsReadOnlySpan();
            var length = transitionFromAny.Length;

            if (length < 1)
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                var transition = transitionFromAny[i];

                if (EqualityComparer<TStateId>.Default.Equals(transition.to, _activeState.name))
                {
                    continue;
                }

                if (TryTransition(transition))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryAllDirectTransitions()
        {
            lock (_idToBundleMap)
            {
                if (_idToBundleMap.TryGetValue(_activeState.name, out var bundle) == false)
                {
                    return false;
                }

                var transitions = bundle.Transitions.Span;
                var length = transitions.Length;

                if (length < 1)
                {
                    return false;
                }

                for (int i = 0; i < length; i++)
                {
                    var transition = transitions[i];

                    if (TryTransition(transition))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private void PerformVerticalTransition()
        {
            StateMachine.StateCanExit();
        }

        #region    EXTENSION_METHODS
        #endregion =================
        private StateBundle<TStateId, TEvent> GetOrCreateStateBundle(TStateId id)
        {
            lock (_idToBundleMap)
            {
                var stateBundleById = _idToBundleMap;

                if (stateBundleById.TryGetValue(id, out var bundle) == false)
                {
                    bundle = new();
                    stateBundleById[id] = bundle;
                }

                return bundle;
            }
        }

        public void Dispose()
        {
            _idToBundleMap.Dispose();
            _triggerTransitionsFromAny.Dispose();
        }
    }
}
