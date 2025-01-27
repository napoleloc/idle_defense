using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Music
{
    public interface IMusicChannel : IAudioChannel
    {
        UniTask PlayMusicAsync(float fadeInTime, CancellationToken token);
    }
}
