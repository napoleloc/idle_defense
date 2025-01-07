using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Musics
{
    public interface IMusicPool
    {
        /// <summary>
        /// 
        /// </summary>
        void PlayMusic();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        UniTask PlayMusicAsync();

        /// <summary>
        /// 
        /// </summary>
        void PauseMusic();

        /// <summary>
        /// 
        /// </summary>
        void UnpauseMusic();

        /// <summary>
        /// 
        /// </summary>
        void StopMusic();

    }
}
