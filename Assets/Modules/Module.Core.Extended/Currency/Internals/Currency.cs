using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Currency.Internals
{
    [System.Serializable]
    internal class Currency
    {
        [EnumToggleButtons]
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private int _defaultValue;
        [SerializeField] private int _value;

        private event Action<int> OnChangedEvent;

        public CurrencyType CurrencyType => _currencyType;
        public int DefaultValue => _defaultValue;
        public int Value
        {
            get => _value;
            set => _value = value;
        }

        public void Initialize()
        {
            _value = _defaultValue;
        }

        public void Reigister(Action<int> listener)
            => OnChangedEvent += listener;

        public void Unregister(Action<int> listener)
            => OnChangedEvent -= listener;

        public void Invoke(int value)
            => OnChangedEvent?.Invoke(value);
    }
}
