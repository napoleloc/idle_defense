using UnityEngine;

namespace Module.Core.HFSM
{
    public interface ITimer
    {
        public float ElapsedTime { get; }

        void Reset();
    }

    public class Timer : ITimer
    {
        public float startTime;
        public float ElapsedTime
        {
            get => Time.time - startTime;
        }

        public void Reset()
        {
            startTime = Time.time;
        }
    }
}
