using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio.Interfaces
{
    public interface IAudioChannelContainer
    {
        UniTask InitializeAsync();
        void Deinitialize();
    }
}
