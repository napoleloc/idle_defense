namespace Module.GameUI
{
    public static class ScreenNames
    {
        private const string PREFAB_FORMAT = "prefab-screen-{0}";

        private const string MAIN_LOBBY_NAME = "mainlobby";
        private const string SHOP_NAME = "shop";
        private const string INVENTORY_NAME = "ability";
        private const string TATLENTS_NAME = "talents";
        private const string IN_GAME_NAME = "ingame";

        public static string MainLobbyScreenName()
            => string.Format(PREFAB_FORMAT, MAIN_LOBBY_NAME);

        public static string ShopScreenName()
            => string.Format(PREFAB_FORMAT, SHOP_NAME);

        public static string AbilityScreenName()
            => string.Format(PREFAB_FORMAT, INVENTORY_NAME);

        public static string TalentsScreenName()
            => string.Format(PREFAB_FORMAT, TATLENTS_NAME);

        public static string InGameScreenName()
            => string.Format(PREFAB_FORMAT, IN_GAME_NAME);
    }
}
