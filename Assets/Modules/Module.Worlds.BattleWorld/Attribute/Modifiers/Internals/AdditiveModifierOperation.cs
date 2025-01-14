using EncosyTower.Modules.Collections;

namespace Module.Worlds.BattleWorld.Attribute.Modifiers.Internals
{
    internal class AdditiveModifierOperation : ModifierOperation
    {
        /// <summary>Calculates the value of additive modifiers.</summary>
        /// <param name="baseValue">The base value of the stat.</param>
        /// <param name="currentValue">The current value of the stat.</param>
        /// <returns>The calculated value of the additive modifiers.</returns
        public override float CalculateModifiersValue(float baseValue, float currentValue)
        {
            var scopedModifiers = ScopedModifers;

            lock (scopedModifiers)
            {
                var modifiers = scopedModifiers.GetValues();
                float additiveModifiersSum = 0f;

                for ( var i = 0; i < modifiers.Length; i++)
                {
                    var span = modifiers[i].AsSpan();
                    for ( var j = 0; j < span.Length; j++)
                    {
                        var modifier = span[j];
                        additiveModifiersSum += modifier.Value;
                    }
                }

                return baseValue * additiveModifiersSum;
            }
        }
    }
}
