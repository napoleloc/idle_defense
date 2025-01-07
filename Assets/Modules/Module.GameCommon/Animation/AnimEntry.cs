using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.GameCommon.Animation
{
    [System.Serializable]
    public struct AnimEntry
    {
        [HorizontalGroup]
        [HideLabel]
        [SerializeField]
        private AnimationClip _animClip;

        [HorizontalGroup]
        [HideLabel]
        [SerializeField]
        private CharAnim _charAnim;
    }
}
