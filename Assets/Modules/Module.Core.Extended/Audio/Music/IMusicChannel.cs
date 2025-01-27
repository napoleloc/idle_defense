using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Music
{
    public interface IMusicChannel : IAudioChannel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeInTime"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        UniTask PlayMusicAsync(float fadeInTime, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeInTime"></param>
        void PlayMusic(float fadeInTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeOutTime"></param>
        void PauseMusic(float fadeOutTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeInTime"></param>
        void UnpauseMusic(float fadeInTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fadeOutTime"></param>
        void StopMusic(float fadeOutTime);
    }
}
