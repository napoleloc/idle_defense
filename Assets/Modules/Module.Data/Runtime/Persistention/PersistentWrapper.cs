using System.Runtime.CompilerServices;
using Module.Data.Runtime.Serialization;
using UnityEngine;

namespace Module.Data.Runtime.Persistention
{
    internal class PersistentWrapper
    {
        public static readonly string PersistentDataPath = Application.persistentDataPath + "/";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Save(RuntimeDataSerializeContainer globalSave, string fileName)
           => SecureDataPersister.JsonSerializeToPathInternal(globalSave, PersistentDataPath + fileName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RuntimeDataSerializeContainer Load(string fileName)
            => SecureDataPersister.JsonDeserializeFromPathInternal<RuntimeDataSerializeContainer>(PersistentDataPath + fileName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Delete(string fileName)
            => SecureDataPersister.DeleteFileAtPathInternal(PersistentDataPath + fileName);
    }
}
