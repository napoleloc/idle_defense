using EncosyTower.Modules.Collections;

namespace Module.Worlds.BattleWorld.Attribute.Modifiers.Internals
{
    internal class FlatModifierOperation : ModifierOperation
    {
        /// <summary>Calculates the value of flat modifiers.</summary>
        /// <param name="baseValue">The base value of the stat.</param>
        /// <param name="currentValue">The current value of the stat.</param>
        /// <returns>The calculated value of the flat modifiers.</returns>
        public override float CalculateModifiersValue(float baseValue, float currentValue)
        {
            var scopedModifiers = ScopedModifers;

            lock (scopedModifiers)
            {
                var modifiers = scopedModifiers.GetValues();
                float flatModifiersSum = 0f;

                for (var i = 0; i < modifiers.Length; i++)
                {
                    var span = modifiers[i].AsSpan();
                    for (var j = 0; j < span.Length; j++)
                    {
                        var modifier = span[j];
                        flatModifiersSum += modifier.Value;
                    }
                }

                return flatModifiersSum;
            }
        }
    }
}
