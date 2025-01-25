using UnityEngine;

namespace Module.Data.Runtime.DataTableAsstes
{
    public interface IRuntimeDataTableAsset { }

    public abstract class RuntimeDataTableAsset : ScriptableObject, IRuntimeDataTableAsset
    {
        internal protected virtual void Initialize() { }
    }
}
