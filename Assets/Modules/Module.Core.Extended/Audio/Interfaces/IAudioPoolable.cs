namespace Module.Core.Extended.Audio.Interfaces
{
    public interface IAudioPoolable
    {
        void OnGetFromPool();
        void OnReturnToPool();
    }
}
