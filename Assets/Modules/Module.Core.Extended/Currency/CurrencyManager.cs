using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Logging;
using Module.Core.Extended.Currency.Internals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Currency
{
    public class CurrencyManager : MonoBehaviour, IDisposable
    {
        private static CurrencyManager s_instance;
        public static CurrencyManager Instance => s_instance;

        [SerializeField, LabelText("Data-base", SdfIconType.Server)]
        [AssetSelector]
        private CurrencyDatabase _db;

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_instance?.Dispose();
            s_instance = null;
        }
#endif

        private void Awake()
        {
            s_instance ??= this;

            _db.Initialize();
        }

        private void OnDestroy()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAmount(CurrencyType type, int amount)
        {
            if(Instance._db.TryGetCurrency(type, out var currency))
            {
                return currency.Value >= amount;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetAmount(CurrencyType type)
        {
            if (Instance._db.TryGetCurrency(type, out var currency))
            {
                return currency.Value;
            }

            DevLoggerAPI.LogError($"type: {type} in valid!");
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReigster(CurrencyType currencyType, Action<int> listener)
        {
            if(Instance._db.TryGetCurrency(currencyType, out var currency))
            {
                currency.Reigister(listener);
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryUnregister(CurrencyType currencyType, Action<int> listener)
        {
            if (Instance._db.TryGetCurrency(currencyType, out var currency))
            {
                currency.Unregister(listener);
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SaveCurrency() => _db.Save();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearCurrency() => _db.Clear();

        public void Dispose()
        {
            
        }
    }
}
