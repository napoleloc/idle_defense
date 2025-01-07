using LitMotion;
using UnityEngine;

namespace Module.GameUI.FloatingUI
{
    public readonly struct AlphaOptions
    {
        public readonly Vector2 Value;
        public readonly float Duration;
        public readonly Ease Easing;

        public AlphaOptions(Vector2 value
            , float duration
            , Ease easing)
        {
            Value = value;
            Duration = duration;
            Easing = easing;
        }
    }
}
