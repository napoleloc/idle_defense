using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Vaults;
using EncosyTower.Modules;
using Module.Core.Extended.PubSub;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;
using EncosyTower.Modules.PubSub;
using UnityEngine.SceneManagement;

namespace Module.Core.Extended.Timing
{
    public sealed class WorldTimer : MonoBehaviour, IDisposable
    {
        public static readonly Id<WorldTimer> PresetId = default;

        public const int HOUR_IN_SECONDS = 60 * 60;

        public readonly static ValueRef<int> HourInSeconds = new(HOUR_IN_SECONDS);

        public readonly static ValueRef<int> DayInSeconds = new(HourInSeconds * 24);

        public readonly static ValueRef<int> WeekInSeconds = new(DayInSeconds * 7);

        public readonly static ValueRef<int> RedundantEpochWeekInSeconds = new(DayInSeconds * 3);

        private static long s_serverTimeDeltaInSeconds = 0;

        public static DateTime Now
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => DateTime.UtcNow.AddSeconds(s_serverTimeDeltaInSeconds);
        }

        public static long NowInSeconds
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (long)(Now - DateTime.UnixEpoch).TotalSeconds;
        }

        public static long NowInDays
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NowInInterval(DayInSeconds);
        }

        public static long NowInWeeks
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NowInInterval(WeekInSeconds);
        }

        public static long NowInHours
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NowInInterval(HourInSeconds);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long NowInInterval(int interval)
            => NowInSeconds / interval;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetTimeInSeconds(DateTime value)
            => (long)(value - DateTime.UnixEpoch).TotalSeconds;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetTimeInHours(DateTime value)
            => GetTimeInInterval(value, HourInSeconds);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetTimeInWeeks(DateTime value)
            => GetTimeInInterval(value, WeekInSeconds);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetTimeInDays(DateTime value)
            => GetTimeInInterval(value, DayInSeconds);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetTimeInInterval(DateTime value, int interval)
            => GetTimeInSeconds(value) / interval;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RemainTimeToNextHour()
            => RemainTimeToNextInterval(HourInSeconds);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RemainTimeToNextDay()
            => RemainTimeToNextInterval(DayInSeconds);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RemainTimeToNextWeek()
            => RemainTimeToNextInterval(WeekInSeconds);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RemainTimeToNextInterval(int interval)
            => interval - NowInSeconds % interval;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetTimeInInterval(DateTime value, int interval, int offsetNow)
            => (GetTimeInSeconds(value) + offsetNow) / interval;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long RemainTimeToNextInterval(int interval, int offsetNow)
            => interval - (NowInSeconds + offsetNow) % interval;

        public static void SetHourInSeconds(int seconds)
        {
            HourInSeconds.Value = seconds;
            DayInSeconds.Value = HourInSeconds * 24;
            WeekInSeconds.Value = DayInSeconds * 7;
        }

        public static void SetDayInSeconds(int seconds)
        {
            DayInSeconds.Value = seconds;
            HourInSeconds.Value = DayInSeconds / 24;
            WeekInSeconds.Value = DayInSeconds * 7;
        }

        public static void SetWeekInSeconds(int seconds)
        {
            WeekInSeconds.Value = seconds;
            DayInSeconds.Value = WeekInSeconds / 7;
            HourInSeconds.Value = DayInSeconds / 24;
        }

        public static void CreateInstance(Scene scene)
        {
            var type = TypeCache.Get<WorldTimer>();
            var go = new GameObject(type.Name, type);
            go.MoveToScene(scene);
        }

        private readonly ArrayMap<Id2, ITimer> _timers = new();
        private readonly Queue<Message> _messageQueue =new();
        private bool _canUpdate;

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            SetHourInSeconds(HOUR_IN_SECONDS);
        }
#endif

        private void Awake()
        {
            var subscriber = WorldMessenger.Subscriber.TimerScope().WithState(this);
            subscriber.Subscribe<RegisterToTimerMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<UnregisterToTimerMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<SetCurrentTimeMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<StopUpdateTimeMessage>(static (state, msg) => state.Handle(msg));

            _canUpdate = true;

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private void Update()
        {
            if (_canUpdate)
            {
                UpdateTime(Time.deltaTime);
            }

            ProcessMessageQueue();
        }

        private void Handle(RegisterToTimerMessage message)
        {
            _messageQueue.Enqueue(new Message {
                id = message.Id
                ,
                timer = message.Timer
                ,
                operation = Operation.Register
            });
        }

        private void Handle(UnregisterToTimerMessage message)
        {
            _messageQueue.Enqueue(new Message {
                id = message.Id
                ,
                timer = default
                ,
                operation = Operation.Unregister
            });
        }

        private void Handle(SetCurrentTimeMessage message)
        {
            s_serverTimeDeltaInSeconds = message.ServerTimeDeltaInSeconds;
        }

        private void Handle(StopUpdateTimeMessage message)
        {
            _canUpdate = message.IsStop;
        }

        private void ProcessMessageQueue()
        {
            var timers = _timers;
            var messageQueue = _messageQueue;

            while (messageQueue.Count > 0)
            {
                var message = messageQueue.Dequeue();

                switch (message.operation)
                {
                    case Operation.Register:
                    {
                        if (message.timer != null)
                        {
                            timers[message.id] = message.timer;
                        }
                        break;
                    }

                    case Operation.Unregister:
                    {
                        timers.Remove(message.id);
                        break;
                    }
                }
            }
        }

        private void UpdateTime(float deltaTime)
        {
            var timers = _timers.GetValues();
            var length = timers.Length;

            for (int i = 0; i < length; i++)
            {
                var timer = timers[i];

                if (timer.Enabled == false)
                {
                    continue;
                }

                timer.RemainTime -= TimeSpan.FromSeconds(deltaTime);

                if (timer.RemainTime <= TimeSpan.Zero)
                {
                    timer.OnTimeZero();

                    if (timer.Enabled)
                    {
                        timer.RemainTime = timer.Interval;
                    }
                    else
                    {
                        timer.RemainTime = TimeSpan.Zero;
                    }
                }
            }
        }

        private enum Operation
        {
            Register = 0,
            Unregister = 1,
        }

        private struct Message
        {
            public Id2 id;
            public ITimer timer;
            public Operation operation;
        }


        public void Dispose()
        {
            _timers.Dispose();
        }
    }
}
