using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Currency
{
    public class CurrencyTest : MonoBehaviour
    {
        [Button]
        private void Test()
        {
            var currencyManager = CurrencyManager.Instance;
        }
    }
}
