using EncosyTower.Modules.Serialization;
using UnityEngine;

namespace Module.Data.GameSave
{
    [System.Serializable]
    public class EntryFile
    {
        [SerializeField] 
        private int _hash;
        [SerializeField] 
        private string _json;
        [System.NonSerialized]
        private IFile _file;

        public int Hash => _hash;
        public IFile File => _file;

        public EntryFile(int hash, IFile file)
        {
            _hash = hash;
            _file = file;
        }

        public void Flush()
        {
            if(_file != default) _file.Flush();
        }

        public void Restore<T>()
            where T : IFile
        {
            JsonHelper.TryDeserialize(_json, out _file);
        }
    }
}
