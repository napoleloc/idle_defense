using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Logging;
using UnityEngine;

namespace Module.Data.Runtime.DataTableAsstes
{
    using RuntimeTableAssetRef = LazyLoadReference<RuntimeDataTableAsset>;

    [CreateAssetMenu(fileName =nameof(RuntimeDatabaseAsset), menuName = "Idle-Defense/Runtime-Data/Runtime Database")]
    public class RuntimeDatabaseAsset : ScriptableObject
    {
        [SerializeField]
        private RuntimeTableAssetRef[] _assetRefs = Array.Empty<RuntimeTableAssetRef>();

        [SerializeField]
        private RuntimeTableAssetRef[] _redundantAssetRefs = Array.Empty<RuntimeTableAssetRef>();

        private readonly Dictionary<string, RuntimeDataTableAsset> _nameToAsset = new Dictionary<string, RuntimeDataTableAsset>();
        private readonly Dictionary<Type, RuntimeDataTableAsset> _typeToAsset = new Dictionary<Type, RuntimeDataTableAsset>();

        protected IReadOnlyDictionary<string, RuntimeDataTableAsset> NameToAsste => _nameToAsset;
        protected IReadOnlyDictionary<Type, RuntimeDataTableAsset> TypeToAsset => _typeToAsset;

        protected ReadOnlyMemory<RuntimeTableAssetRef> AssetRefs => _assetRefs;
        protected ReadOnlyMemory<RuntimeTableAssetRef> RedundantAssetRefs => _redundantAssetRefs;

        public bool Initialized { get; private set; }

        public void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            var assetRefs = AssetRefs.Span;
            var assetsLenght = assetRefs.Length;
            var nameToAsset = _nameToAsset;
            var typeToAsset = _typeToAsset;

            nameToAsset.Clear();
            nameToAsset.EnsureCapacity(assetsLenght);

            typeToAsset.Clear();
            typeToAsset.EnsureCapacity(assetsLenght);

            for (var i = 0; i < assetsLenght; i++)
            {
                var assetRef = assetRefs[i];

                if (assetRef.isSet == false || assetRef.isBroken)
                {
                    LogErrorReferenceIsInvalid(i, this);
                    continue;
                }

                var asset = assetRef.asset;

                if (asset == false)
                {
                    LogErrorAssetIsInvalid(i, this);
                    continue;
                }

                var type = asset.GetType();
                nameToAsset[type.Name] = asset;
                typeToAsset[type] = asset;

                asset.Initialize();
            }

            Initialized = true;
        }

        public void Deinitialize()
        {
            if(Initialized == false)
            {
                return;
            }

            Initialized = false;

            foreach (var asset in _nameToAsset.Values)
            {

            }

            _nameToAsset.Clear();
            _typeToAsset.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetRuntimeDataTableAsset([NotNull] string name, out RuntimeDataTableAsset tableAsset)
        {
            if(Initialized == false)
            {
                LogErrorDatabaseIsNotInitialized(this);
                tableAsset = null;
                return false;
            }

            if(_nameToAsset.TryGetValue(name, out var asset))
            {
                tableAsset = asset;
                return true;
            }
            else
            {
                LogErrorCannotFindAsset(name, this);
            }

            tableAsset = null;
            return false;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetRuntimeDataTableAsset([NotNull] Type type, out RuntimeDataTableAsset tableAsset)
        {
            if(Initialized == false)
            {
                LogErrorDatabaseIsNotInitialized(this);
                tableAsset = null;
                return false;
            }

            if(_typeToAsset.TryGetValue(type, out var asset))
            {
                tableAsset = asset;
                return true;
            }
            else
            {
                LogErrorCannotFindAsset(type, this);
            }

            tableAsset = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetRuntimeDataTableAsset<T>(out T tableAsset)
            where T : RuntimeDataTableAsset
        {
            if(Initialized == false)
            {
                LogErrorDatabaseIsNotInitialized(this);
                tableAsset = null;
                return false;
            }

            var type = typeof(T);

            if(_typeToAsset.TryGetValue(type, out var asset))
            {
                if(asset is T assetT)
                {
                    tableAsset = assetT;
                    return true;
                }
                else
                {
                    LogErrorFoundAssetIsNotValidType<T>(asset);
                }
            }
            else
            {
                LogErrorCannotFindAsset(type, this);
            }

            tableAsset = null;
            return false;
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorReferenceIsInvalid(int index, RuntimeDatabaseAsset context)
        {
            DevLoggerAPI.LogError(context, $"Runtime Table Asset reference at index {index} is invalid.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorAssetIsInvalid(int index, RuntimeDatabaseAsset context)
        {
            DevLoggerAPI.LogError(context, $"Runtime Table Asset at index {index} is invalid.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorDatabaseIsNotInitialized(RuntimeDatabaseAsset context)
        {
            DevLoggerAPI.LogError(context, $"The runtime database is not initialized yet. Please invoke {nameof(Initialize)} method beofre using.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorCannotFindAsset(string name, RuntimeDatabaseAsset context)
        {
            DevLoggerAPI.LogError(context, $"Cannot find any runtime data table asset named {name}.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorCannotFindAsset(Type type, RuntimeDatabaseAsset context)
        {
            DevLoggerAPI.LogError(context, $"Cannot find any runtime data table asset of type {type}.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorFoundAssetIsNotValidType<T>(RuntimeDataTableAsset context)
        {
            DevLoggerAPI.LogError(context, $"The runtime data table asset is not an instance of {typeof(T)}");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorFoundAssetIsNotValidType(Type type, RuntimeDataTableAsset context)
        {
            DevLoggerAPI.LogError(context, $"The runtime data table asset is not an instance of {type}");
        }
    }
}
