namespace Module.Core.Extended.Currency
{
    public static class CurrencyNames
    {
        private const string FLY_FORMAT = "prefab-currency-fly-{0}";

        private const string COINNAME = "1";
        private const string GEMNAME = "2";

        public static string CoinFlyName()
            => string.Format(FLY_FORMAT, COINNAME);

        public static string GemFlyName()
            => string.Format(FLY_FORMAT, GEMNAME);
    }
}
