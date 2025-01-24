using Module.Worlds.BattleWorld.Attribute;

namespace Module.Data.Runtime.Talents
{
    public struct TalentIdData
    {
        private AttributeKind _kind;
        private AttributeType _subId;

        public AttributeKind Kind => _kind;
        public AttributeType SubId => _subId;
    }

    [System.Serializable]
    public struct TalentEntry
    {
        private TalentIdData _id;
        private uint _level;

        public TalentIdData Id => _id;
        public uint Level => _level;
    }
}
