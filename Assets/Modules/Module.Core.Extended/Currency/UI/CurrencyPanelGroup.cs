using System;
using System.Collections.Generic;
using EncosyTower.Modules.Collections;
using UnityEngine;

namespace Module.Core.Extended.Currency.UI
{
    public class CurrencyPanelGroup : MonoBehaviour
    {
        private static readonly Dictionary<CurrencyType, int> s_typeToIndexMap = new();

        private CurrencyPanel[] _group;

        private void OnValidate()
        {
            if (_group.IsNullOrEmpty())
            {
                _group = GetComponentsInChildren<CurrencyPanel>();
            }
        }

        public void Initialize()
        {
            var map = s_typeToIndexMap;
            var group = _group.AsSpan();

            map.Clear();
            map.EnsureCapacity(group.Length);

            for (int i = 0; i < group.Length; i++)
            {
                var type = group[i].CurrencyType;

                if(map.TryAdd(type, i) == false)
                {
                    continue;
                }

                group[i].Initialize();
            }
        }

        public void Cleanup()
        {
            var group = _group.AsSpan();

            for (int i = 0;i < group.Length; i++)
            {
                group[i].Cleanup();
            }
        }
    }
}
