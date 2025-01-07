namespace Module.GameUI
{
    public static class ModalNames
    {
        private const string PREFAB_FORMAT = "prefab-modal-{0}";

        #region    INGAME_MODALS
        #endregion =============

        private const string PAUSE_NAME = "pause";
        private const string DEFEAT_NAME = "defeat";
        private const string VICTORY_NAME = "victory";

        public static string PauseModalName()
            => string.Format(PREFAB_FORMAT, PAUSE_NAME);

        public static string DefeatModalName()
            => string.Format(PREFAB_FORMAT, DEFEAT_NAME);

        public static string VictoryModalName()
            => string.Format(PREFAB_FORMAT, VICTORY_NAME);

        #region    MAINLOBBY_MODALS
        #endregion ================

        private const string SETTINGS_NAME = "setting";
        private const string MAIL_NAME = "mail";

        public static string SettingsModalName()
            => string.Format(PREFAB_FORMAT, SETTINGS_NAME);

        public static string MailModalName()
            => string.Format(PREFAB_FORMAT, MAIL_NAME);
    }
}
