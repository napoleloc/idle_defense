using Module.Worlds.BattleWorld.Attribute;

namespace Module.Data.Runtime.Serialization.Talents
{
    public readonly struct RuntimeTalentIdData
    {
        public readonly AttributeKind Kind;
        public readonly AttributeType Type;
    }

    public struct RuntimeTalentDataEntry
    {
        public RuntimeTalentIdData Id { get; init; }
        public int Level { get; init; }
    }
}
