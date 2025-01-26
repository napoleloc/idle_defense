using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Music
{
    public class MusicChannelContainer : AudioChannelContainer<MusicChannel>
    {
        public async UniTask PlayMusicAsync(
            AudioType audioType
            , float fadeInTime = 0
            , CancellationToken token = default)
            => await PlayMusicAsyncInternal(audioType, fadeInTime, token);
        
        public void PlayMusic(
            AudioType audioType
            , float fadeInTime = 0
            , CancellationToken token = default)
            => PlayMusicAndForget(audioType, fadeInTime, token).Forget();

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
                musicChannel.Initialize(clip);
                await musicChannel.PlayMusicAsync(fadeInTime, token);
            }
        }
    }
}
