using System.Threading;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.Audio.Interfaces;

namespace Module.Core.Extended.Audio.Sound
{
    public interface ISoundChannel : IAudioChannel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeInTime"></param>
        void PlaySound(float fadeInTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeInTime"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        UniTask PlaySoundAsync(float fadeInTime, CancellationToken token = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeOutTime"></param>
        void PauseSound(float fadeOutTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeInTime"></param>
        void UnpauseSound(float fadeInTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeOutTime"></param>
        void StopSound(float fadeOutTime);
    }
}
