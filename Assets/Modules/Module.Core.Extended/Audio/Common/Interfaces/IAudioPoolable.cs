namespace Module.Core.Extended.Audio
{
    public interface IAudioPoolable
    {
        /// <summary>
        /// 
        /// </summary>
        void OnGetFromPool();
        
        /// <summary>
        /// 
        /// </summary>
        void OnReturnToPool();
    }
}
