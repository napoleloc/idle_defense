using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using Module.Core.HFSM.States;
using Module.Core.HFSM.Transitions;

namespace Module.Core.HFSM.Internals
{
    internal class StateBundle<TStateId, TEvent> : IDisposable
        where TStateId : unmanaged
        where TEvent : unmanaged
    {
        private readonly ArrayMap<TEvent, FasterList<Transition<TStateId>>> _eventToTransitionMap;
        private readonly FasterList<Transition<TStateId>> _transitions = new();
        private State<TStateId> _state;

        public ReadOnlyMemory<Transition<TStateId>> Transitions
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _transitions.AsReadOnlyMemory();
        }

        public ArrayMap<TEvent, FasterList<Transition<TStateId>>> EventToTransitionMap
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _eventToTransitionMap;
        }

        public State<TStateId> State { get => _state; set => _state = value; }

        public StateBundle()
        {
            _transitions = new();
            _eventToTransitionMap = new();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Transition<TStateId> transition)
        {
            _transitions.Add(transition);
        }

        public void Add(TEvent trigger, Transition<TStateId> transition)
        {
            lock (_eventToTransitionMap)
            {
                var map = _eventToTransitionMap;

                if(map.TryGetValue(trigger, out var transitions) == false)
                {
                    transitions = new();
                    map[trigger] = transitions;
                }

                transitions.Add(transition);
            }
        }

        public bool TryGet(TEvent trigger, out Span<Transition<TStateId>> transitions)
        {
            if (_eventToTransitionMap.ContainsKey(trigger))
            {
                transitions = _eventToTransitionMap[trigger].AsSpan();
                return true;
            }

            transitions = default;
            return false;
        }

        public void Dispose()
        {
            _transitions.Clear();
            _eventToTransitionMap.Dispose();
        }
    }
}
