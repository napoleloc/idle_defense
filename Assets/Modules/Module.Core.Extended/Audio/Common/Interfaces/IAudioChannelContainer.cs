using Cysharp.Threading.Tasks;

namespace Module.Core.Extended.Audio
{
    public interface IAudioChannelContainer
    {
        UniTask InitializeAsync();
        void Deinitialize();
    }
}
