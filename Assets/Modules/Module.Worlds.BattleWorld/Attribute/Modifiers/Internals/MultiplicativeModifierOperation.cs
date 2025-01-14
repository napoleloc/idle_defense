namespace Module.Worlds.BattleWorld.Attribute.Modifiers.Internals
{
    using EncosyTower.Modules.Collections;

    internal class MultiplicativeModifierOperation : ModifierOperation
    {
        /// <summary>Calculates the value of multiplicative modifiers.</summary>
        /// <param name="baseValue">The base value of the stat.</param>
        /// <param name="currentValue">The current value of the stat.</param>
        /// <returns>The calculated value of the multiplicative modifiers.</returns>
        public override float CalculateModifiersValue(float baseValue, float currentValue)
        {
            var scopedModifiers = ScopedModifers;

            lock (scopedModifiers)
            {
                var modifiers = scopedModifiers.GetValues();
                float calculatedValue = currentValue;

                for (var i = 0; i < modifiers.Length; i++)
                {
                    var span = modifiers[i].AsSpan();
                    for (var j = 0; j < span.Length; j++)
                    {
                        var modifier = span[j];
                        calculatedValue += calculatedValue * modifier.Value;
                    }
                }

                return calculatedValue - currentValue;
            }
        }
    }
}
