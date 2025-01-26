using UnityEngine.Audio;

namespace Module.Core.Extended.Audio.Interfaces
{
    public interface IAudioChannel
    {
        void SetAudioMixerGroup(AudioMixerGroup mixerGroup);
        void SetVolume(float volume);
        void SetLoop(bool loop);
    }
}
