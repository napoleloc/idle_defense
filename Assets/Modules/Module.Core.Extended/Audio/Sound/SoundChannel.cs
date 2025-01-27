using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Sound
{
    public class SoundChannel : AudioChannel, ISoundChannel
    {
        public async UniTask PlaySoundAsync(float fadeInTime, CancellationToken token = default)
            => await PlaySoundAsync(fadeInTime, token);

        public void PlaySound(float fadeInTime)
            => PlaySoundAndForget(fadeInTime).Forget();

        public void PauseSound(float fadeOutTime)
        {
            if (paused)
            {
                return;
            }

            audioSource.Pause();
            paused = true;
        }

        public void UnpauseSound(float fadeInTime)
        {
            if (paused == false)
            {
                return;
            }

            paused = false;
            audioSource.UnPause();
        }

        public void StopSound(float fadeOutTime)
        {
            if (audioSource.isPlaying)
            {
                return;
            }

            audioSource.Stop();
        }

        private UniTaskVoid PlaySoundAndForget(float fadeInTime, CancellationToken token = default)
            => PlaySoundAndForget(fadeInTime, token);

        private async UniTask PlaySoundAsyncInternal(float fadeInTime, CancellationToken token)
        {
            if (Validate() == false)
            {
                return;
            }

            await UniTask.WaitUntil(() => audioSource.isPlaying, cancellationToken: token);
        }
    }
}
