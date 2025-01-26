using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Sound
{
    public class SoundChannel : AudioChannel, ISoundChannel
    {
        public UniTask PlaySoundAsync(float fadeInTime, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public void PlaySound(float fadeInTime)
        {
            throw new System.NotImplementedException();
        }

        public void PauseSound(float fadeOutTime)
        {
            throw new System.NotImplementedException();
        }

        public void StopSound(float fadeOutTime)
        {
            throw new System.NotImplementedException();
        }

        public void UnpauseSound(float fadeInTime)
        {
            throw new System.NotImplementedException();
        }
    }
}
