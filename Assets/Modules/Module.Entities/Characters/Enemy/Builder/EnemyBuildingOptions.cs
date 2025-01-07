using Module.GameCommon;

namespace Module.Entities.Characters.Enemy.Builder
{
    public readonly struct EnemyBuildingOptions
    {
        public readonly EnemyType Type;
        public readonly EnemyId SubId;

        public EnemyBuildingOptions(EnemyType type, EnemyId subId)
        {
            this.Type = type;
            this.SubId = subId;
        }

        public EnemyBuildingOptions(EnemyType type, MinionId subId)
        {
            Type = type;
            SubId = EnemyIdExtensions.ToEnemyId(subId);
        }

        public EnemyBuildingOptions(EnemyType type, EliteId subId)
        {
            Type = type;
            SubId = EnemyIdExtensions.ToEnemyId(subId);
        }
    }
}
