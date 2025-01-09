using Module.Core.HFSM.States;
using Module.Core.HFSM.Transitions;
using System.Runtime.CompilerServices;
using System;

namespace Module.Core.HFSM
{
    public static class StateMachineExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddState<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TStateId id
            , bool needsExitTime = false
            , bool iGhostState = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var state = new State<TStateId, TEvent>(needsExitTime, iGhostState);
            self.AddState(id, state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTransition<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TStateId from
            , TStateId to
            , Predicate<TransitionCondition<TStateId>> condition = null
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = CreateOptimizedTransition(from, to, condition, beforeTransition, afterTransition, forceInstantly);
            self.AddTransition(transition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTransitionFromAny<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TStateId to
            , Predicate<TransitionCondition<TStateId>> condition = null
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = CreateOptimizedTransition(default, to, condition, beforeTransition, afterTransition, forceInstantly);
            self.AddTransitionFromAny(transition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTriggerTransition<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TEvent trigger
            , TStateId from
            , TStateId to
            , Predicate<TransitionCondition<TStateId>> condition = null
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = CreateOptimizedTransition(from, to, condition, beforeTransition, afterTransition, forceInstantly);
            self.AddTriggerTransition(trigger, transition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTriggerTransitionFromAny<TOwnId, TStateId, TEvent>(
           this StateMachine<TOwnId, TStateId, TEvent> self
           , TEvent trigger
           , TStateId to
           , Predicate<TransitionCondition<TStateId>> condition = null
           , Action<TransitionCondition<TStateId>> beforeTransition = null
           , Action<TransitionCondition<TStateId>> afterTransition = null
           , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = CreateOptimizedTransition(default, to, condition, beforeTransition, afterTransition, forceInstantly);
            self.AddTriggerTransitionFromAny(trigger, transition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTwoWayTransitio<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TStateId from
            , TStateId to
            , Predicate<TransitionCondition<TStateId>> condition
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstanly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = new TransitionCondition<TStateId>(from, to, condition, beforeTransition,afterTransition, forceInstanly);
            self.AddTwoWayTransition(transition);
        }

        public static void AddTwoWayTriggerTransition<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TEvent trigger
            , TStateId from
            , TStateId to
            , Predicate<TransitionCondition<TStateId>> condition
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = new TransitionCondition<TStateId>(from, to, condition, beforeTransition,afterTransition,forceInstantly);
            self.AddTwoWayTriggerTransition(trigger, transition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddExitTransition<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TStateId from
            , Predicate<TransitionCondition<TStateId>> condition = null
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = CreateOptimizedTransition(from, default, condition, beforeTransition, afterTransition, forceInstantly);
            self.AddExitTransition(transition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddExitTransitionFromAny<TOwnId, TStateId, TEvent>(
             this StateMachine<TOwnId, TStateId, TEvent> self
             , Predicate<TransitionCondition<TStateId>> condition = null
             , Action<TransitionCondition<TStateId>> beforeTransition = null
             , Action<TransitionCondition<TStateId>> afterTransition = null
             , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = CreateOptimizedTransition(default, default, condition, beforeTransition, afterTransition, forceInstantly);
            self.AddExitTransitionFromAny(transition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddExitTriggerTransition<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TEvent trigger
            , TStateId from
            , Predicate<TransitionCondition<TStateId>> condition = null
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = CreateOptimizedTransition(from, default, condition, beforeTransition, afterTransition, forceInstantly);
            self.AddExitTriggerTransition(trigger, transition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddExitTriggerTransitionFromAny<TOwnId, TStateId, TEvent>(
            this StateMachine<TOwnId, TStateId, TEvent> self
            , TEvent trigger
            , Predicate<TransitionCondition<TStateId>> condition = null
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
        ) where TOwnId : unmanaged
            where TStateId : unmanaged
            where TEvent : unmanaged
        {
            var transition = CreateOptimizedTransition(default, default, condition, beforeTransition, afterTransition, forceInstantly);
            self.AddExitTriggerTransitionFromAny(trigger, transition);
        }

        private static Transition<TStateId> CreateOptimizedTransition<TStateId>(
            TStateId from
            , TStateId to
            , Predicate<TransitionCondition<TStateId>> condition = null
            , Action<TransitionCondition<TStateId>> beforeTransition = null
            , Action<TransitionCondition<TStateId>> afterTransition = null
            , bool forceInstantly = false
        ) where TStateId : unmanaged
        {
            if (condition == null && beforeTransition == null && afterTransition == null)
            {
                return new(from, to, forceInstantly);
            }

            return new TransitionCondition<TStateId>(from, to, condition, beforeTransition, afterTransition, forceInstantly);
        }
    }
}
