using UnityEngine;

namespace Module.Data.Runtime
{
    public interface IRuntimeDataTableAsset { }

    public abstract class RuntimeDataTableAsset : ScriptableObject, IRuntimeDataTableAsset
    {
        internal protected virtual void Initialize() { }

        internal protected virtual void Deinitialize() { }
    }
}
