using System.Diagnostics.CodeAnalysis;
using Module.Core.Extended.Audio.Interfaces;
using UnityEngine;
using UnityEngine.Audio;

namespace Module.Core.Extended.Audio
{
    public class AudioChannel : MonoBehaviour, IAudioChannel, IAudioPoolable
    {
        protected AudioSource source;

        public void Initialize([NotNull] AudioClip clip)
        {
            source.clip = clip;
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
