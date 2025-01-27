using UnityEngine.Audio;

namespace Module.Core.Extended.Audio
{
    public interface IAudioChannel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mixerGroup"></param>
        void SetAudioMixerGroup(AudioMixerGroup mixerGroup);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="fadeInTime"></param>
        void SetVolume(float volume, float fadeInTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loop"></param>
        void SetLoop(bool loop);
    }
}
