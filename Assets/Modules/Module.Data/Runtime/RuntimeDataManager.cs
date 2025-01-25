using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Logging;
using Module.Data.Runtime.Serialization;
using UnityEngine;

namespace Module.Data.Runtime
{
    public static class RuntimeDataManager
    {
        private const string FILE_NAME = "runtime-data";

        private static readonly Dictionary<string, RuntimeDataSerialize> s_nameToData = new();
        private static readonly Dictionary<Type, RuntimeDataSerialize> s_typeToData = new();
        private static RuntimeDataSerializeContainer s_container;

        private static bool s_initialized;

        public static void Initialize()
        {
            if (s_initialized)
            {
                return;
            }

            var entries = s_container.Entries.Span;
            var entriesLenght = entries.Length;
            var nameToData = s_nameToData;
            var typeToData = s_typeToData;

            nameToData.Clear();
            nameToData.EnsureCapacity(entriesLenght);

            typeToData.Clear();
            nameToData.EnsureCapacity(entriesLenght);

            for (var i = 0; i < entriesLenght; i++)
            {
                var entry = entries[i];

                var type = entry.GetType();
                nameToData[type.Name] = entry;
                typeToData[type] = entry;

                entry.Serialize();
            }

            s_initialized = true;
        }

        public static void Deinitialize()
        {
            if(s_initialized == false)
            {
                return;
            }

            s_initialized = false;

            foreach (var entry in s_container.Entries.Span)
            {
                entry.Deserialize();
            }

            s_nameToData.Clear();
            s_typeToData.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetData([NotNull] string name, out RuntimeDataSerialize data)
        {
            if(s_initialized == false)
            {
                LogErrorRuntimeDataIsNotInitialized();
                data = null;
                return false;
            }

            if(s_nameToData.TryGetValue(name, out var weakRef))
            {
                data = weakRef;
                return true;
            }
            else
            {
                LogErrorCannotFindAsset(name);
            }

            data = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetData([NotNull] Type type, out RuntimeDataSerialize data)
        {
            if (s_initialized == false)
            {
                LogErrorRuntimeDataIsNotInitialized();
                data = null;
                return false;
            }

            if(s_typeToData.TryGetValue(type, out var weakRef))
            {
                data = weakRef;
                return true;
            }
            else
            {
                LogErrorCannotFindAsset(type);
            }

            data = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetData<T>(out T data)
            where T : RuntimeDataSerialize
        {
            if(s_initialized == false)
            {
                LogErrorRuntimeDataIsNotInitialized();
                data = null;
                return false;
            }

            var type = typeof(T);

            if(s_typeToData.TryGetValue(type,out var weakRef))
            {
                if(weakRef is T weakRefT)
                {
                    data = weakRefT;
                    return true;
                }
                else
                {
                    LogErrorFoundAssetIsNotValidType<T>();
                }
            }
            else
            {
                LogErrorCannotFindAsset(type);
            }

            data = null;
            return false;
        }

        private static void InitializeAndClearInternal()
        {

        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorRuntimeDataIsNotInitialized()
        {
            DevLoggerAPI.LogError($"The runtime Data is not initialized yet. Please invoke {nameof(Initialize)} method beofre using.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorCannotFindAsset(string name)
        {
            DevLoggerAPI.LogError($"Cannot find any runtime data serialize named {name}.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorCannotFindAsset(Type type)
        {
            DevLoggerAPI.LogError($"Cannot find any runtime data serialize of type {type}.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorFoundAssetIsNotValidType<T>()
        {
            DevLoggerAPI.LogError($"The runtime data serialize is not an instance of {typeof(T)}");
        }
    }
}

