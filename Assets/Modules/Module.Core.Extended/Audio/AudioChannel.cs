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

        protected UniTask uniTask;
        protected CancellationTokenSource unitCts;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool Validate()
           => audioSource && audioSource.clip;

        protected void RenewUnitCts()
        {
            if(gameObject == false || this == false)
            {
                return;
            }

            unitCts ??= new();
            if (unitCts.IsCancellationRequested)
            {
                unitCts.Dispose();
                unitCts = new();
            }
        }

        protected async UniTask SmoothChangeVolume(
            float targetVolume
            , float duration
            , CancellationToken token
        )
        {
            float remainTime = duration;

            while (remainTime > 0.01F)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                volume = Mathf.Lerp(volume, targetVolume, Time.deltaTime / remainTime);
                audioSource.volume = volume;
                remainTime -= Time.deltaTime;

                await UniTask.WaitForEndOfFrame(token);

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
            => audioSource.clip = clip;

        public void SetAudioMixerGroup(AudioMixerGroup mixerGroup)
            => audioSource.outputAudioMixerGroup = mixerGroup;

        public void SetVolume(float volume, float fadeInTime = 0)
        {
            this.volume = volume;
            audioSource.volume = volume;
        }

        public void SetLoop(bool loop)
        {
            looping = loop;
            audioSource.loop = loop;
        }

        public virtual void OnGetFromPool()
        {

        }

        public virtual void OnReturnToPool()
        {
            unitCts.Cancel();
        }
    }
}
