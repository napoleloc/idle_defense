using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules;
using EncosyTower.Modules.Logging;
using EncosyTower.Modules.Vaults;
using UnityEngine;

namespace Module.Data.Runtime
{
    using TableRef = LazyLoadReference<DataTable>;

    public class GameDataRumtimeManager : MonoBehaviour
    {
        public static readonly Id<GameDataRumtimeManager> PresetId = default;

        [SerializeField]
        internal TableRef[] _assetRefs = Array.Empty<TableRef>();

        private readonly Dictionary<string, DataTable> _nameToAsset = new();
        private readonly Dictionary<Type, DataTable> _typeToAsset = new();

        protected ReadOnlyMemory<TableRef> AssetRefs => _assetRefs;

        public bool Initialized { get; protected set; }

        private void Awake()
        {
            Initialize();

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void OnDestroy()
        {
            Deinitialize();

            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }


        private void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            var assetRefs = AssetRefs.Span;
            var assetsLength = assetRefs.Length;
            var nameToAsset = _nameToAsset;
            var typeToAsset = _typeToAsset;

            nameToAsset.Clear();
            nameToAsset.EnsureCapacity(assetsLength);

            typeToAsset.Clear();
            typeToAsset.EnsureCapacity(assetsLength);

            for (var i = 0; i < assetsLength; i++)
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

        public virtual void Deinitialize()
        {
            if (Initialized == false)
            {
                return;
            }

            Initialized = false;

            foreach (var asset in _nameToAsset.Values)
            {
                asset.Deinitialize();
            }

            _nameToAsset.Clear();
            _typeToAsset.Clear();
        }

        public bool TryGetDataTableAsset([NotNull] string name, out DataTable tableAsset)
        {
            if (Initialized == false)
            {
                LogErrorDatabaseIsNotInitialized(this);
                tableAsset = null;
                return false;
            }

            if (_nameToAsset.TryGetValue(name, out var asset))
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
        public bool TryGetDataTableAsset([NotNull] Type type, out DataTable tableAsset)
        {
            if (Initialized == false)
            {
                LogErrorDatabaseIsNotInitialized(this);
                tableAsset = null;
                return false;
            }

            if (_typeToAsset.TryGetValue(type, out var asset))
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
        public bool TryGetDataTableAsset<T>(out T tableAsset)
            where T : DataTable
        {
            if (Initialized == false)
            {
                LogErrorDatabaseIsNotInitialized(this);
                tableAsset = null;
                return false;
            }

            var type = typeof(T);

            if (_typeToAsset.TryGetValue(type, out var asset))
            {
                if (asset is T assetT)
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

        public bool TryGetDataTableAsset<T>([NotNull] string name, out T tableAsset)
            where T : DataTable
        {
            if (Initialized == false)
            {
                LogErrorDatabaseIsNotInitialized(this);
                tableAsset = null;
                return false;
            }

            if (_nameToAsset.TryGetValue(name, out var asset))
            {
                if (asset is T assetT)
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
                LogErrorCannotFindAsset(name, this);
            }

            tableAsset = null;
            return false;
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorReferenceIsInvalid(int index, GameDataRumtimeManager context)
        {
            DevLoggerAPI.LogError(context, $"Table Asset reference at index {index} is invalid.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorAssetIsInvalid(int index, GameDataRumtimeManager context)
        {
            DevLoggerAPI.LogError(context, $"Table Asset at index {index} is invalid.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorDatabaseIsNotInitialized(GameDataRumtimeManager context)
        {
            DevLoggerAPI.LogError(context, $"The database is not initialized yet. Please invoke {nameof(Initialize)} method beofre using.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorCannotFindAsset(string name, GameDataRumtimeManager context)
        {
            DevLoggerAPI.LogError(context, $"Cannot find any data table asset named {name}.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorCannotFindAsset(Type type, GameDataRumtimeManager context)
        {
            DevLoggerAPI.LogError(context, $"Cannot find any data table asset of type {type}.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorFoundAssetIsNotValidType<T>(DataTable context)
        {
            DevLoggerAPI.LogError(context, $"The data table asset is not an instance of {typeof(T)}");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorFoundAssetIsNotValidType(Type type, DataTable context)
        {
            DevLoggerAPI.LogError(context, $"The data table asset is not an instance of {type}");
        }
    }
}
