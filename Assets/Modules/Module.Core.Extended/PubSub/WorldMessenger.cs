using EncosyTower.Modules.PubSub;
using UnityEngine;

namespace Module.Core.Extended.PubSub
{
    public static class WorldMessenger
    {
        public static MessagePublisher Publisher => Instance.MessagePublisher;
        public static MessageSubscriber Subscriber => Instance.MessageSubscriber;

        public static Messenger Instance => s_instance ??= new();


        private static Messenger s_instance;

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_instance?.Dispose();
            s_instance = null;
        }
#endif
    }
}
