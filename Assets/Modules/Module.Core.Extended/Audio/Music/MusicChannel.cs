using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Music
{
    public class MusicChannel : AudioChannel, IMusicChannel
    {
        public async UniTask PlayMusicAsync(float fadeInTime = 0, CancellationToken token = default)
        {
            await UniTask.NextFrame(token);
        }
    }
}
