using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Logging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Currency.Internals
{
    [CreateAssetMenu]
    internal class CurrencyDatabase : ScriptableObject
    {
        private readonly Dictionary<CurrencyType, int> _typeToIndexMap = new();

        [SerializeField] private Currency[] _currencies;

        public ReadOnlyMemory<Currency> Currencies
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _currencies;
        }

        public void Initialize()
        {
            var map = _typeToIndexMap;
            var currencies = Currencies.Span;

            map.Clear();
            map.EnsureCapacity(currencies.Length);

            for (var i = 0; i < currencies.Length; i++)
            {
                var type = currencies[i].CurrencyType;

                if (type.HasData()) 
                    currencies[i].Value = type.GetAmountFromData();
                else
                {
                    type.SetAmountToData(currencies[i].DefaultValue);
                    currencies[i].Initialize();
                }

                if(map.TryAdd(type, i) == false)
                {
                    continue;
                }
            }
        }

        public bool TryGetCurrency(CurrencyType type, out Currency currency)
        {
            var result = _typeToIndexMap.TryGetValue(type, out var index);
            currency = result ? Currencies.Span[index] : default;
            return result;
        }

        public void Save()
        {
            var currencies = Currencies.Span;

            for (int i = 0; i < currencies.Length; i++)
            {
                var currency = currencies[i];
                currency.CurrencyType.SetAmountToData(currency.Value);
            }
        }

        [Button(buttonSize:30, Icon = SdfIconType.Trash)]
        public void Clear()
        {
            var currencies = Currencies.Span;

            for (int i = 0; i < currencies.Length; i++)
            {
                var currency = currencies[i];
                currency.CurrencyType.ClearData();
                currency.CurrencyType.SetAmountToData(currency.DefaultValue);
                currency.Initialize();
            }
        }
    }
}
