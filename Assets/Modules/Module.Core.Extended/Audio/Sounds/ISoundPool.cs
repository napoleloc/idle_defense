using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Sounds
{
    public interface ISoundPool
    {
        /// <summary>
        /// 
        /// </summary>
        void PlaySound();
        
        /// <summary>
        /// 
        /// </summary>
        UniTask PlaySoundAsync();
        
        /// <summary>
        /// 
        /// </summary>
        void PauseSound();

        /// <summary>
        /// 
        /// </summary>
        void UnpauseSound();

        /// <summary>
        /// 
        /// </summary>
        void StopSound();
    }
}
