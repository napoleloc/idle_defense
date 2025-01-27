using System;
using System.Collections.Generic;
using EncosyTower.Modules.Logging;
using System.Diagnostics;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Module.Entities.Tower.Data
{
    using ConfigAssetRef = LazyLoadReference<TowerConfigAsset>;

    [CreateAssetMenu(fileName = nameof(TowerDatabaseAsset), menuName = "Idle-Defense/Entities/Tower/Database")]
    public class TowerDatabaseAsset : ScriptableObject
    {
        [SerializeField]
        private ConfigAssetRef[] _assetRefs;

        private readonly Dictionary<string, TowerConfigAsset> _nameToAsset = new();
        private readonly Dictionary<Type, TowerConfigAsset> _typeToAsset = new();

        public ReadOnlyMemory<ConfigAssetRef> AssetRefs => _assetRefs;
        public IReadOnlyDictionary<string, TowerConfigAsset> NameToAsset => _nameToAsset;
        public IReadOnlyDictionary<Type, TowerConfigAsset> TypeToAsset => _typeToAsset;
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

            for ( var i = 0; i < assetsLenght; i++)
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
            }

            Initialized = true;
        }

        public void Deinitialized()
        {
            if(Initialized == false)
            {
                return;
            }

            _nameToAsset.Clear();
            _typeToAsset.Clear();
        }

        public bool TryGetConfigAsset(string name, out TowerConfigAsset configAsset)
        {
            if(Initialized == false)
            {
                configAsset = null;
                return false;
            }

            if(_nameToAsset.TryGetValue(name, out var asset))
            {
                configAsset = asset;
                return true;
            }
            else
            {

            }

            configAsset = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetConfigAsset(Type type, out TowerConfigAsset configAsset)
        {
            if(Initialized == false)
            {
                configAsset = null;
                return false;
            }

            if(_typeToAsset.TryGetValue(type, out var asset))
            {
                configAsset = asset;
                return true;
            }
            else
            {

            }

            configAsset = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetConfigAsset<T>(out T configAsset)
            where T : TowerConfigAsset
        {
            if(Initialized == false)
            {
                configAsset = null;
                return false;
            }

            var type = typeof(T);

            if(_typeToAsset.TryGetValue(type, out var asset))
            {
                if(asset is T aseetT)
                {
                    configAsset = aseetT;
                    return true;
                }
                else
                {

                }
            }
            else
            {

            }

            configAsset = null;
            return false;
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorReferenceIsInvalid(int index, TowerDatabaseAsset context)
        {
            DevLoggerAPI.LogError(context, $"Runtime Table Asset reference at index {index} is invalid.");
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorAssetIsInvalid(int index, TowerDatabaseAsset context)
        {
            DevLoggerAPI.LogError(context, $"Runtime Table Asset at index {index} is invalid.");
        }

    }
}
