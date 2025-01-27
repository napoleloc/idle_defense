using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Module.Core.Extended.Audio
{
    public class AudioChannel : MonoBehaviour, IAudioChannel, IAudioPoolable
    {
        protected bool looping;
        protected bool paused;
        protected float volume;
        protected AudioSource audioSource;
        protected AsyncLazy asyncLazy;

        public void Test()
        {
            var task = SmoothChangeVolume(0, 0, default);
            if(task.Status == UniTaskStatus.Succeeded)
            {

            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool Validate()
           => audioSource && audioSource.clip;

        private async UniTask SmoothChangeVolume(float targetVolume, float duration, CancellationToken token)
        {
            while (duration > 0.01F)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                volume = Mathf.Lerp(volume, targetVolume, Time.deltaTime / duration);
                await UniTask.WaitForEndOfFrame(token);
                duration -= Time.deltaTime;

                if (token.IsCancellationRequested)
                {
                    break;
                }
            }

            if(token.IsCancellationRequested)
            {
                return;
            }

            volume = targetVolume;
        }

        public void Initialize([NotNull] AudioClip clip)
        {
            audioSource.clip = clip;
        }

        public void SetAudioMixerGroup(AudioMixerGroup mixerGroup)
        {

        }

        public void SetLoop(bool loop)
        {

        }

        public void SetVolume(float volume)
        {

        }

        public virtual void OnGetFromPool()
        {

        }

        public virtual void OnReturnToPool()
        {

        }
    }
}
