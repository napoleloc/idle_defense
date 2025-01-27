using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Music
{
    public class MusicChannel : AudioChannel, IMusicChannel
    {
        internal MusicChannelContainer _container;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask PlayMusicAsync(float fadeInTime = 0, CancellationToken token = default)
            => await PlayMusicAsyncInternal(fadeInTime, token);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PlayMusic(float fadeInTime = 0)
            => PlayMusicAndForget(fadeInTime, default).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseMusic(float fadeOutTime = 0)
            => PauseMusicAndForget(fadeOutTime).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnpauseMusic(float fadeInTime = 0)
            => UnpauseMusicAndForget(fadeInTime).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopMusic(float fadeOutTime = 0)
            => StopMusicAndForget(fadeOutTime).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTaskVoid PlayMusicAndForget(float fadeInTime, CancellationToken token)
            => await PlayMusicAsyncInternal(fadeInTime, token);

        private async  UniTask PlayMusicAsyncInternal(float fadeInTime, CancellationToken token)
        {
            if(Validate() == false)
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
        private async UniTaskVoid PauseMusicAndForget(float fadeOutTime)
            => await PauseMusicAsyncInternal(fadeOutTime);

        private async UniTask PauseMusicAsyncInternal(float fadeOutTime, CancellationToken token = default)
        {
            if (paused)
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
                await SmoothChangeVolume(1, fadeOutTime, token);
            }

            audioSource.Pause();
            paused = true;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        private async UniTask UnpauseMusicAndForget(float fadeInTime)
            => await UnpauseMusicAsyncInternal(fadeInTime);

        private async UniTask UnpauseMusicAsyncInternal(float fadeInTime, CancellationToken token = default)
        {
            if(paused == false)
            {
                return;
            }

            paused = false;
            audioSource.volume = 0;
            audioSource.UnPause();

            if (fadeInTime > 0)
            {
                if (uniTask.Status != UniTaskStatus.Succeeded)
                {
                    unitCts.Cancel();
                }

                RenewUnitCts();
                await SmoothChangeVolume(1, fadeInTime, token);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTaskVoid StopMusicAndForget(float fadeOutTime)
            => await StopMusicAsyncInternal(fadeOutTime);

        private async UniTask StopMusicAsyncInternal(float fadeOutTime, CancellationToken token = default)
        {
            if(audioSource.isPlaying == false)
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
                await SmoothChangeVolume(1, fadeOutTime, token);
            }
        }
    }
}
