using UnityEngine;

namespace Module.Data.Runtime
{
    public interface IDataTable { }

    public abstract class DataTable : ScriptableObject, IDataTable
    {
        internal abstract void SetEntries(object obj);

        internal protected virtual void Initialize() { }

        internal protected virtual void Deinitialize() { }
    }
}
