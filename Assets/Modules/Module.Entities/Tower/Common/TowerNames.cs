using Module.Entities.Tower.Data;
using Module.GameCommon;

namespace Module.Entities.Tower
{
    public static class TowerNames
    {
        public const string PREFAB_FORMAT = "prefab-building-tower-{0}-{1}";

        public static string Format(TowerKind kind, TowerType type)
            => string.Format(PREFAB_FORMAT, kind.ToStringFast(), type.ToStringFast());
    }
}
