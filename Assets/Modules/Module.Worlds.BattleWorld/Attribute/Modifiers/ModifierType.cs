namespace Module.Worlds.BattleWorld.Attribute.Modifiers
{
    /// <summary>
    /// Defines the types of modifiers that can be applied to a stat.
    /// The order of the enumeration values determines the order in which they are calculated.
    /// </summary>
    public enum ModifierType : byte
    {
        /// <summary>A flat modifier that adds a fixed value to the current value.</summary>
        Flat = 1,

        /// <summary>An additive modifier that adds a percentage of the base value to the current value.</summary>
        Additive = 2,

        /// <summary>A multiplicative modifier that adds a percentage of the current value to the current value.</summary>
        Multiplicative = 3
    }

}
