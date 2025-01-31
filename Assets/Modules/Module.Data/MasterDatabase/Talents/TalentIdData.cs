using EncosyTower.Modules.Data;
using Module.Worlds.BattleWorld.Attribute;

namespace Module.Data.MasterDatabase.Talents
{
    public partial struct TalentIdData : IData
    {
        [DataProperty]
        public AttributeKind Kind { get => Get_Kind(); init => Set_Kind(value); }

        [DataProperty]
        public AttributeType SubId { get => Get_SubId(); init => Set_SubId(value); }
    }
}
