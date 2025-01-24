namespace Module.Data.Runtime.Serialization
{
    public interface IRuntimeDataSerializable
    {
        void Serialize();
        void Deserialize();
    }

    public class RuntimeDataSerialize : IRuntimeDataSerializable
    {
        public void Serialize()
        {
            throw new System.NotImplementedException();
        }

        public void Deserialize()
        {
            throw new System.NotImplementedException();
        }
    }
}
