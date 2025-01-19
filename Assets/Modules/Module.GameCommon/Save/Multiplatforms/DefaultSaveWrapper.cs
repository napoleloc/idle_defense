using System.Runtime.CompilerServices;

namespace Module.GameCommon.Save.Multiplatforms
{
    public class DefaultSaveWrapper : BaseSaveWrapper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Save(FileTable globalSave, string fileName)
            => JsonSerializeToPathInternal(globalSave, PersistentDataPath + fileName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override FileTable Load(string fileName)
            => JsonDeserializeFromPathInternal(PersistentDataPath + fileName, logIfFileNotExists: false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Delete(string fileName)
            => DeleteFileAtPathInternal(PersistentDataPath + fileName);
        
    }
}
