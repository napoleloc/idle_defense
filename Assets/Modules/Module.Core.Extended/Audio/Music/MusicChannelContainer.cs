using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Music
{
    public class MusicChannelContainer : AudioChannelContainer<MusicChannel>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask PlayMusicAsync(
            AudioType audioType
            , float fadeInTime = 0
            , CancellationToken token = default)
            => await PlayMusicAsyncInternal(audioType, fadeInTime, token);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PlayMusic(
            AudioType audioType
            , float fadeInTime = 0
            , CancellationToken token = default)
            => PlayMusicAndForget(audioType, fadeInTime, token).Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseMusic(AudioType audioType, float fadeOutTime = 0)
        {
            if (TypeToChannel.TryGetValue(audioType, out var channel))
            {
                channel.PauseMusic(fadeOutTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnpauseMusic(AudioType audioType, float fadeInTime = 0)
        {
            if (TypeToChannel.TryGetValue(audioType, out var channel))
            {
                channel.UnpauseMusic(fadeInTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopMusic(AudioType audioType, float fadeOutTime = 0)
        {
            if (TypeToChannel.TryGetValue(audioType, out var channel))
            {
                channel.StopMusic(fadeOutTime);
            }
        }

        private async UniTaskVoid PlayMusicAndForget(
            AudioType audioType
            , float fadeInTime
            , CancellationToken token)
            => await PlayMusicAsyncInternal(audioType , fadeInTime, token);

        private async UniTask PlayMusicAsyncInternal(
            AudioType audioType
            , float fadeInTime
            , CancellationToken token
        )
        {
            var musicChannel = Pool.RentComponent(true);
            if (LoaderAsset.TryGetClip(audioType, out var clip))
            {
                musicChannel._container = this;
                musicChannel.AudioType = audioType;
                musicChannel.Initialize(clip);
                await musicChannel.PlayMusicAsync(fadeInTime, token);
            }
        }
    }
}
