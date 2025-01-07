using LitMotion;
using UnityEngine;

namespace Module.GameUI.FloatingUI
{
    public readonly struct AnchordOptions
    {
        public readonly Vector2 OriginPosition;
        public readonly Vector2 FinalPosition;
        public readonly float Duration;
        public readonly Ease Easing;

        public AnchordOptions(Vector2 startPosition
            , Vector2 finalPosition
            , float duration
            , Ease easing)
        {
            OriginPosition = startPosition;
            FinalPosition = finalPosition; 
            Duration = duration;
            Easing = easing;
        }
    }
}
