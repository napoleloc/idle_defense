using EncosyTower.Modules.EnumExtensions;

namespace Module.Worlds.BattleWorld.Attribute
{
    [EnumExtensions]
    public enum AttributeKind : byte { Offensive, Defense }
    
    [EnumTemplate]
    public partial struct AttributeType_Template { }

    [EnumMembersForTemplate(typeof(AttributeType_Template), 0)]
    [EnumExtensions]
    public enum OffensiveAttributeType : byte
    {
        Damage,
        AttackSpeed,
        Range,
    }

    [EnumMembersForTemplate(typeof(AttributeType_Template), 50)]
    [EnumExtensions]
    public enum DefenseAttributeType : byte
    {
        Hp,
        Regen,
        Armor,
    }
}
