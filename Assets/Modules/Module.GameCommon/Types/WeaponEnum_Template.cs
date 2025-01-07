using EncosyTower.Modules.EnumExtensions;

namespace Module.GameCommon
{
    public enum WeaponKind : byte { WeaponRanged, WeaponMelee }

    [EnumTemplate]
    public partial struct WeaponId_Template { }

    [EnumMembersForTemplate(typeof(WeaponId_Template), 0)]
    [EnumExtensions]
    public enum WeaponMeleeId : byte
    {

    }

    [EnumMembersForTemplate(typeof(WeaponId_Template), 100)]
    [EnumExtensions]
    public enum WeaponRangedId : byte
    {

    }
}
