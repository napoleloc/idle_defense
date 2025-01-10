using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules;
using Module.GameCommon.Animation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Characters
{
    [Serializable]
    public struct AnimEntry
    {
        [HideLabel]
        [OnValueChanged("OnValueChanged")]
        public AnimationClip clip;
        [HideLabel]
        [ReadOnly]
        public string animName;
        [HideLabel]
        public CharAnim anim;

#if UNITY_EDITOR
        private void OnValueChanged()
        {
            animName = clip.name;
        }
#endif
    }

    public class CharacterAnimationComponent : MonoBehaviour, IEntityComponent
    {
        private readonly Dictionary<CharAnim, int> _animToIndexMap = new Dictionary<CharAnim, int>();
        private readonly Dictionary<CharAnim, int> _animToHashMap = new Dictionary<CharAnim, int>();

        [TableList]
        [SerializeField]
        private AnimEntry[] _anims;
        private Animator _animator;

        public Animator Animator => _animator;
        public ReadOnlyMemory<AnimEntry> Anims
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _anims;
        }

        public void InitializeComponent()
        {
            var animToIndexMap = _animToIndexMap;
            var animToHashMap = _animToHashMap;
            var anims = _anims.AsSpan();

            animToIndexMap.Clear();
            animToHashMap.Clear();
            animToIndexMap.EnsureCapacity(anims.Length);
            animToHashMap.EnsureCapacity(anims.Length);

            for ( int i = 0; i < anims.Length; i++)
            {
                var anim = anims[i];

                if (anim.clip.IsInvalid())
                {
                    continue;
                }

                animToIndexMap[anim.anim] = i;
                animToHashMap[anim.anim] = Animator.StringToHash(anim.clip.name);
            }
        }

        public void InitializeDependencies()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void CrossFadeAnim(CharAnim anim)
        {
            if(TryGet(anim, out int hash))
            {
                _animator.CrossFade(hash, 0.1F, 0);
            }
        }

        public bool TryGetAnimatorStateInfo(CharAnim anim, ref AnimatorStateInfo animatorState)
        {
            if(TryGet(anim, out AnimEntry entry))
            {
                return TryGetAnimatorStateInfoInternal(entry.animName, 0, ref animatorState);
            }

            return false;
        }

        private bool TryGet(CharAnim anim, out AnimEntry entry)
        {
            var result = _animToIndexMap.TryGetValue(anim, out var index);
            entry = result ? Anims.Span[index] : default;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGet(CharAnim anim, out int hash)
        {
            var result = _animToHashMap.TryGetValue(anim, out hash);
            return result;
        }

        private bool TryGetAnimatorStateInfoInternal(string animName, int layer, ref AnimatorStateInfo stateInfo)
        {
            if (string.IsNullOrEmpty(animName))
            {
                stateInfo = default;
                return false;
            }

            var currentState =  _animator.GetCurrentAnimatorStateInfo(layer);
            bool flag = currentState.IsName(animName);

            stateInfo = flag ? currentState : default;
            return flag;
        }

    }
}
