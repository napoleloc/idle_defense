using System;
using Module.GameCommon.Animation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Characters
{
    [Serializable]
    public struct AnimEntry
    {
        [HideLabel]
        public CharAnim anim;
        [HideLabel]
        public AnimationClip clip;
    }

    public class CharacterAnimationComponent : MonoBehaviour, IEntityComponent
    {
        [TableList]
        [SerializeField]
        private AnimEntry[] _anims;

        private Animator _animator;

        public Animator Animator => _animator;

        public void InitializeDependencies()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void CrossFadeAnim()
        {
            
        }
    }
}
