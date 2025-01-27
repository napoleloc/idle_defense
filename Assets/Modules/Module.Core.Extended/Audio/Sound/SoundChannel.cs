using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Sound
{
    public class SoundChannel : AudioChannel, ISoundChannel
    {
        internal SoundChannelContainer _container;

        public async UniTask PlaySoundAsync(float fadeInTime = 0, CancellationToken token = default)
            => await PlaySoundAsync(fadeInTime, token);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PlaySound(float fadeInTime = 0)
            => PlaySoundAndForget(fadeInTime).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseSound(float fadeOutTime = 0)
            => PauseSoundAndForget(fadeOutTime).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnpauseSound(float fadeInTime = 0)
            => UnpauseSoundAndFoget(fadeInTime).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopSound(float fadeOutTime = 0)
            => StopSoundAndForget(fadeOutTime).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private UniTaskVoid PlaySoundAndForget(float fadeInTime, CancellationToken token = default)
            => PlaySoundAndForget(fadeInTime, token);

        private async UniTask PlaySoundAsyncInternal(float fadeInTime, CancellationToken token)
        {
            if (Validate() == false)
            {
                return;
            }

            audioSource.Play();

            if(fadeInTime > 0)
            {
                if(uniTask.Status != UniTaskStatus.Succeeded)
                {
                    unitCts.Cancel();
                }

                RenewUnitCts();
                await SmoothChangeVolume(1, fadeInTime, token);
            }

            await UniTask.WaitUntil(() => audioSource.isPlaying == false, cancellationToken: token);

            OnReturnToPool();
            _container.Pool.Return(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTaskVoid PauseSoundAndForget(float fadeOutTime)
            => await PauseSoundAsyncInternal(fadeOutTime);

        private async UniTask PauseSoundAsyncInternal(float fadeOutTime)
        {
            if (paused)
            {
                return;
            }

            if(fadeOutTime > 0)
            {
                if (uniTask.Status != UniTaskStatus.Succeeded)
                {
                    unitCts.Cancel();
                }

                RenewUnitCts();
                await SmoothChangeVolume(0, fadeOutTime, unitCts.Token);
            }

            audioSource.Pause();
            paused = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTaskVoid UnpauseSoundAndFoget(float fadeInTime)
            => await UnpauseSoundAsyncInternal(fadeInTime);

        private async UniTask UnpauseSoundAsyncInternal(float fadeInTime)
        {
            if(paused == false)
            {
                return;
            }

            audioSource.volume = 0;
            audioSource.UnPause();

            if (fadeInTime > 0)
            {
                if (uniTask.Status != UniTaskStatus.Succeeded)
                {
                    unitCts.Cancel();
                }

                RenewUnitCts();
                await SmoothChangeVolume(1, fadeInTime, unitCts.Token);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTaskVoid StopSoundAndForget(float fadeOutTime)
            => await StopSoundAsyncInternal(fadeOutTime);

        private async UniTask StopSoundAsyncInternal(float fadeOutTime)
        {
            if (audioSource.isPlaying)
            {
                return;
            }

            if (fadeOutTime > 0)
            {
                if (uniTask.Status != UniTaskStatus.Succeeded)
                {
                    unitCts.Cancel();
                }

                RenewUnitCts();
                await SmoothChangeVolume(0, fadeOutTime, unitCts.Token);
            }

            audioSource.Stop();

            OnReturnToPool();
            _container.Pool.Return(this);
        }
    }
}
