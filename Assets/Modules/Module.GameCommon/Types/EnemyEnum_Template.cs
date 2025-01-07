using EncosyTower.Modules.EnumExtensions;

namespace Module.GameCommon
{
    public enum EnemyType : byte { Minion, Elite,}

    [EnumTemplate]
    public partial struct EnemyId_Template { }

    [EnumMembersForTemplate(typeof(EnemyId_Template), 0)]
    [EnumExtensions]
    public enum MinionId : byte {minion_1 }

    [EnumMembersForTemplate(typeof (EnemyId_Template), 100)]
    [EnumExtensions]
    public enum EliteId : byte {elite_1 }
}
