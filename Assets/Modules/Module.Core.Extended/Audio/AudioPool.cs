using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace Module.Core.Extended.Audio
{
    public abstract class AudioPool : MonoBehaviour
    {
        protected AudioSource audioSource;
        protected AudioMixerGroup mixerGroup;
        protected AudioClip audioClip;

        protected float volume;
        protected bool isLoop;
        protected bool isPlaying;
        protected bool isPaused;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
            => audioSource = gameObject.GetOrAddComponent<AudioSource>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool Validate() 
            => audioSource && audioClip;
        

        public virtual void Initialize(
            float volume
            , bool isLoop
            , AudioClip audioClip
            , AudioMixerGroup mixerGroup
        )
        {
            this.volume = volume;
            this.isLoop = isLoop;

            this.mixerGroup = mixerGroup;
            this.audioClip = audioClip;

            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.clip = this.audioClip;
            audioSource.volume = this.volume;
            audioSource.loop = this.isLoop;
            audioSource.Play();

            isPlaying = audioSource.isPlaying;
            isPaused = audioSource.isPlaying == false;
        }

        public void SetAudioMixerGroup(AudioMixerGroup mixerGroup)
        {
            this.mixerGroup = mixerGroup;

            audioSource.outputAudioMixerGroup = this.mixerGroup;
        }

        public void SetVolume(float volume)
        {
            this.volume = volume;

            audioSource.volume = this.volume;
        }

        public void SetLoop(bool loop)
        {
            isLoop = loop;

            audioSource.loop = isLoop;
        }

    }
}
