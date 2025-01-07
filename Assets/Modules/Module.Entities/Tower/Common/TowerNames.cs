namespace Module.Entities.Tower
{
    public static class TowerNames
    {
        public const string PREFAB_FORMAT = "prefab-tower-{0}";

        public static string Format(ushort id)
            => string.Format(PREFAB_FORMAT, id);
    }
}
