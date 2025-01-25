namespace Module.Data.Runtime.Serialization
{
    public interface IRuntimeDataSerializable
    {
        void Serialize();
        void Deserialize();
    }

    public class RuntimeDataSerialize : IRuntimeDataSerializable
    {
        public virtual void Serialize() { }

        public virtual void Deserialize() { }
    }
}
