namespace Module.Core.Extended.Audio
{
    public interface IAudioPoolable
    {
        void OnGetFromPool();
        void OnReturnToPool();
    }
}
