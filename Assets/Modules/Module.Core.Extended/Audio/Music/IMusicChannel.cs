using System.Threading;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.Audio.Interfaces;

namespace Module.Core.Extended.Audio.Music
{
    public interface IMusicChannel : IAudioChannel
    {
        UniTask PlayMusicAsync(float fadeInTime, CancellationToken token);
    }
}
