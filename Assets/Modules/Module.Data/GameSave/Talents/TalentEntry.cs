using Module.Worlds.BattleWorld.Attribute;

namespace Module.Data.GameSave.Talents
{
    [System.Serializable]
    public struct TalentEntry
    {
        public AttributeKind kind;
        public AttributeType type;
    }
}
