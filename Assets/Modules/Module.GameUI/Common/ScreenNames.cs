namespace Module.GameUI
{
    public static class ScreenNames
    {
        private const string PREFAB_FORMAT = "prefab-screen-{0}";

        private const string MAIN_LOBBY_NAME = "main-lobby";
        private const string SHOP_NAME = "shop";
        private const string INVENTORY_NAME = "inventory";
        private const string IN_GAME_NAME = "ingame";

        public static string MainLobbyScreenName()
            => string.Format(PREFAB_FORMAT, MAIN_LOBBY_NAME);

        public static string ShopScreenName()
            => string.Format(PREFAB_FORMAT, SHOP_NAME);

        public static string InventoryScreenName()
            => string.Format(PREFAB_FORMAT, INVENTORY_NAME);

        public static string InGameScreenName()
            => string.Format(PREFAB_FORMAT, IN_GAME_NAME);
    }
}
