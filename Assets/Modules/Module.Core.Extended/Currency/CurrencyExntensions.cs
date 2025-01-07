using System.Runtime.CompilerServices;
using UnityEngine;

namespace Module.Core.Extended.Currency
{
    public static class CurrencyExntensions
    {
        public static bool HasData(this CurrencyType currencyType)
        {
            var result = PlayerPrefs.GetInt(string.Format("has_currency_data_{0}", currencyType.GetHashCode()), 0) <= 0 ? false : true;

            if(result == false)
            {
                PlayerPrefs.SetInt(string.Format("has_currency_data_{0}", currencyType.GetHashCode()), 1);
            }

            return result;
        }

        public static void ClearData(this CurrencyType type)
            => PlayerPrefs.SetInt(string.Format("has_currency_data_{0}", type.GetHashCode()), 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetAmountFromData(this CurrencyType currencyType)
            => PlayerPrefs.GetInt(string.Format("currency_{0}", currencyType.GetHashCode()), 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAmountToData(this CurrencyType currencyType, int amount)
            => PlayerPrefs.SetInt(string.Format("currency_{0}", currencyType.GetHashCode()), amount);
    }
}
