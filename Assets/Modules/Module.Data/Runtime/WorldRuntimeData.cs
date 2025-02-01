using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules;
using EncosyTower.Modules.Logging;
using Module.Data.Runtime.DataTableAsstes;
using Module.Data.Runtime.Persistention;
using Module.Data.Runtime.Serialization;
using UnityEngine;

namespace Module.Data.Runtime
{
    public static class WorldRuntimeData
    {
        private const string FILE_NAME = "runtime-data";
        private const string RESOURCE_PATH = nameof(RuntimeDatabaseAsset);

        private static readonly Dictionary<Type, RuntimeDataSerialize> s_typeToData = new();
        private static PersistentWrapper s_instance;
        private static RuntimeDataSerializeContainer s_container;
        private static RuntimeDatabaseAsset s_runtimeDatabase;

        public static bool Initialized { get; private set; }
        internal static PersistentWrapper Instance => s_instance ??= new();
        public static RuntimeDatabaseAsset RuntimeDatabase
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if(s_runtimeDatabase == false)
                {
                    var handle = new ResourceKey<RuntimeDatabaseAsset>(RESOURCE_PATH);
                    var runtimeDatabaseOpt = handle.TryLoad();

                    if (runtimeDatabaseOpt.HasValue)
                    {
                        s_runtimeDatabase = runtimeDatabaseOpt.ValueOrDefault();
                        DevLoggerAPI.LogInfo($"[WorldRuntimeData] {s_runtimeDatabase.GetType().Name} loaded successfully.");
                    }
                    else
                    {
                        DevLoggerAPI.LogError($"[WorldRuntimeData] RuntimeDatabaseAsset not found at path: 'Resources/{RESOURCE_PATH}'. " +
                                  "Ensure the asset exists and is in the correct Resources folder.");
                    }
                }

                return s_runtimeDatabase;
            }
        }

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_instance = null;
            s_container = null;

            if (s_runtimeDatabase)
            {
                s_runtimeDatabase.Deinitialize();
                s_runtimeDatabase = null;
            }
        }
#endif
        public static void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            s_container = Instance.Load(FILE_NAME);
            s_runtimeDatabase.Initialize();

            var entries = s_container.Entries.Span;
            var entriesLenght = entries.Length;
            var typeToData = s_typeToData;

            typeToData.Clear();
            typeToData.EnsureCapacity(entriesLenght);

            for (var i = 0; i < entriesLenght; i++)
            {
                var entry = entries[i];

                var type = entry.GetType();
                typeToData[type] = entry;

                entry.Serialize();
            }

            Initialized = true;
        }

        public static void Deinitialize()
        {
            if(Initialized == false)
            {
                return;
            }

            Initialized = false;

            foreach (var entry in s_container.Entries.Span)
            {
                entry.Deserialize();
            }

            s_typeToData.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetRuntimeData([NotNull] Type type, out RuntimeDataSerialize data)
        {
            if (Initialized == false)
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
        public static bool TryGetRuntimeData<T>(out T data)
            where T : RuntimeDataSerialize
        {
            if(Initialized == false)
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

