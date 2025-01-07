using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Camera
{
    [CreateAssetMenu]
    internal class CameraConfigAsset : ScriptableObject
    {
        private const string CAMERA_SHIFT = "Shift Settings";
        private const string CAMERA_FORWARD_SHIFT = $"{CAMERA_SHIFT}/Forward Shift";
        private const string CAMERA_TARGET_SHIFT = $"{CAMERA_SHIFT}/Target Shift";

        private const int WIDTH_LABEL = 70;

        [BoxGroup(CAMERA_SHIFT, centerLabel: true)]
        [HorizontalGroup(CAMERA_FORWARD_SHIFT)]
        [LabelWidth(WIDTH_LABEL)]
        [LabelText("Shift X")]
        public float forwardShiftX = 4f;
        [HorizontalGroup(CAMERA_FORWARD_SHIFT)]
        [LabelWidth(WIDTH_LABEL)]
        [LabelText("Shift Z")]
        public float forwardShiftZ = 1f;
        [HorizontalGroup(CAMERA_FORWARD_SHIFT)]
        [LabelWidth(WIDTH_LABEL)]
        [LabelText("Multiplier")]
        public float forwardShiftLerpMultiplier = 4f;

        [HorizontalGroup(CAMERA_TARGET_SHIFT)]
        [LabelWidth(WIDTH_LABEL)]
        [LabelText("Shift X")]
        public float targetShiftX = 4f;
        [HorizontalGroup(CAMERA_TARGET_SHIFT)]
        [LabelWidth(WIDTH_LABEL)]
        [LabelText("Shift Z")]
        public float targetShiftZ = 1f;
        [HorizontalGroup(CAMERA_TARGET_SHIFT)]
        [LabelWidth(WIDTH_LABEL)]
        [LabelText("Multiplier")]
        public float targetShiftLerpMultiplier = 4f;
    }
}
