using System;
using Module.GameCommon;

namespace Module.Entities.Characters.Enemy
{
    public static class EnemyNames
    {
        private const string PREFAB_FORMAT = "prefab-character-{0}";

        public static string GetNameFromId<TId>(TId id)
        {
            switch (id)
            {
                case MinionId minionId:
                    return MinionName(minionId);
                case EliteId eliteId:
                    return EliteName(eliteId);
                default:
                    throw new ArgumentException($"Unsupported ID type: {typeof(TId)}", nameof(id));
            }
        }

        public static string MinionName(MinionId minionId)
        {
            return minionId switch {
                MinionId.minion_1 => string.Format(PREFAB_FORMAT, minionId.ToStringFast()),
                _ => default(string),
            };
        }

        public static string EliteName(EliteId eliteId)
        {
            return eliteId switch {
                EliteId.elite_1 => string.Format(PREFAB_FORMAT, eliteId.ToStringFast()),
                _ => default(string)
            };
        }
    }
}
