using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Sound
{
    public class SoundChannelContainer : AudioChannelContainer<SoundChannel>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask PlaySoundAsync(
            AudioType type
            , float fadeInTime = 0
            , CancellationToken token = default)
            => await PlaySoundAsyncInternal(type, fadeInTime, token);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PlaySound(
            AudioType type
            , float fadeInTime = 0
            , CancellationToken token = default)
            => PlaySoundAndForget(type, fadeInTime, token).Forget();

        private async UniTaskVoid PlaySoundAndForget(
            AudioType type
            , float fadeInTime
            , CancellationToken token)
            => await PlaySoundAsyncInternal(type, fadeInTime, token);

        private async UniTask PlaySoundAsyncInternal(
            AudioType type
            , float fadeInTime
            , CancellationToken token
        )
        {
            var soundChannel = Pool.RentComponent(true);

            if(LoaderAsset.TryGetClip(type, out var clip))
            {
                soundChannel._container = this;
                soundChannel.Initialize(clip);
                await soundChannel.PlaySoundAsync(fadeInTime, token);
            }
        }
    }
}
