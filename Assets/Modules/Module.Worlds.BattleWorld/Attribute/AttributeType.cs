using EncosyTower.Modules.EnumExtensions;

namespace Module.Worlds.BattleWorld.Attribute
{
    [EnumTemplate]
    public partial struct AttributeType_Template { }

    [EnumMembersForTemplate(typeof(AttributeType_Template), 0)]
    public enum OffensiveAttributeType : byte
    {
        AttackSpeed,
    }

    [EnumMembersForTemplate(typeof(AttributeType_Template), 50)]
    public enum DefenseAttributeType : byte
    {

    }
}
