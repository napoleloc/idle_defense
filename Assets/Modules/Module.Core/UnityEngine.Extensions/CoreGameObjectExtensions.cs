using System.Runtime.CompilerServices;
using UnityEngine;

namespace Module.Core
{
    public static class CoreGameObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddComponent<T>(this GameObject self)
            where T : Component
        {
            if (self.TryGetComponent<T>(out var component) == false)
            {
                component = self.AddComponent<T>();
            }

            return component;
        }

        public static T GetOrAddComponent<T>(this Component self)
            where T : Component
        {
            if (self.TryGetComponent<T>(out var component) == false)
            {
                component = self.gameObject.AddComponent<T>();
            }

            return component;
        }

        public static bool TryGetOrAddComponent<T>(this GameObject self, out T component)
            where T : Component
        {
            if (self.TryGetComponent<T>(out component) == false)
            {
                component = self.gameObject.AddComponent<T>();
            }

            return component;
        }

        public static bool TryGetOrAddComponent<T>(this Component self, out T component)
            where T : Component
        {
            if (self.TryGetComponent<T>(out component) == false)
            {
                component = self.gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}
