using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Module.GameUI.FloatingUI
{
    using Random = UnityEngine.Random;

    public static class FloatingUIVault
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MotionSequenceBuilder FloatingVertical(
            [NotNull] RectTransform rectTransform
            , [NotNull] CanvasGroup canvasGroup
            , AnchordOptions anchordOptions
            , AlphaOptions alphaOptions
            , Action onComplete = default
         )
        {
            return FloatingVerticalInternal(rectTransform, canvasGroup, anchordOptions, alphaOptions, onComplete);
        }

        private static MotionSequenceBuilder FloatingVerticalInternal(
            [NotNull] RectTransform rectTransform
            , [NotNull] CanvasGroup canvasGroup
            , AnchordOptions anchordOptions
            , AlphaOptions alphaOptions
            , Action onComplete = default
        )
        {
            rectTransform.anchoredPosition = anchordOptions.OriginPosition;
            canvasGroup.alpha = alphaOptions.Value.x;

            var sequence = LSequence.Create();

            sequence.Append(LMotion.Create(anchordOptions.OriginPosition, anchordOptions.FinalPosition, anchordOptions.Duration)
                .WithEase(anchordOptions.Easing)
                .BindToAnchoredPosition(rectTransform));

            sequence.Append(LMotion.Create(alphaOptions.Value.x, alphaOptions.Value.y, alphaOptions.Duration)
                .WithEase(anchordOptions.Easing)
                .BindToAlpha(canvasGroup));

            return sequence;
        }

        private static MotionSequenceBuilder FloatingCurveInternal([NotNull] RectTransform rectTransform
            , [NotNull] CanvasGroup canvasGroup
            , AnchordOptions anchordOptions
            , AlphaOptions alphaOptions
            , float radius = 250
            , Action onComplete = default
        )
        {
            rectTransform.anchoredPosition = anchordOptions.OriginPosition;
            canvasGroup.alpha = alphaOptions.Value.x;

            Vector2 positionCircle = anchordOptions.OriginPosition + Random.insideUnitCircle * radius;

            var sequence = LSequence.Create();

            return sequence;
        }
    }
}
