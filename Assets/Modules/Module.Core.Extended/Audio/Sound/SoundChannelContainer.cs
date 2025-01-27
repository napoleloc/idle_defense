using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Sound
{
    public class SoundChannelContainer : AudioChannelContainer<SoundChannel>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask PlaySoundAsync(
            AudioType audioType
            , float fadeInTime = 0
            , CancellationToken token = default)
            => await PlaySoundAsyncInternal(audioType, fadeInTime, token);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PlaySound(
            AudioType audioType
            , float fadeInTime = 0
            , CancellationToken token = default)
            => PlaySoundAndForget(audioType, fadeInTime, token).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseSound(AudioType audioType, float fadeOutTime = 0)
        {
            if(TypeToChannel.TryGetValue(audioType, out var channel))
            {
                channel.PauseSound(fadeOutTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnpauseSound(AudioType audioType, float fadeInTime = 0)
        {
            if (TypeToChannel.TryGetValue(audioType, out var channel))
            {
                channel.UnpauseSound(fadeInTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopSound(AudioType audioType, float fadeOutTime = 0)
        {
            if (TypeToChannel.TryGetValue(audioType, out var channel))
            {
                channel.StopSound(fadeOutTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async UniTaskVoid PlaySoundAndForget(
            AudioType type
            , float fadeInTime
            , CancellationToken token)
            => await PlaySoundAsyncInternal(type, fadeInTime, token);

        private async UniTask PlaySoundAsyncInternal(
            AudioType audioType
            , float fadeInTime
            , CancellationToken token
        )
        {
            var soundChannel = GetFromPool(audioType);

            if(LoaderAsset.TryGetClip(audioType, out var clip))
            {
                soundChannel._container = this;
                soundChannel.AudioType = audioType;
                soundChannel.Initialize(clip);
                await soundChannel.PlaySoundAsync(fadeInTime, token);
            }
        }
    }
}
