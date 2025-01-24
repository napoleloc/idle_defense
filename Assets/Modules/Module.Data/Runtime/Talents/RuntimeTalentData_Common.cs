using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Data.Runtime.Talents
{
    [System.Serializable]
    public struct TalentIdData
    {
        [SerializeField]
        private AttributeKind _kind;
        [SerializeField]
        private AttributeType _subId;

        public AttributeKind Kind => _kind;
        public AttributeType SubId => _subId;
    }

    [System.Serializable]
    public struct TalentEntry
    {
        [SerializeField]
        [InlineProperty]
        private TalentIdData _id;
        [SerializeField]
        private uint _level;

        public TalentIdData Id => _id;
        public uint Level => _level;
    }
}
