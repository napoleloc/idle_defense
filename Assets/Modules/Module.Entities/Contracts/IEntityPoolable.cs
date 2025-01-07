namespace Module.Entities
{
    public interface IEntityPoolable
    {
        void OnGetFromPool();
        void OnReturnToPool();
    }
}
