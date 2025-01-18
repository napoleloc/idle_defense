using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Collections.Unsafe;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Data.GameSave
{
    [System.Serializable]
    public class FileTable : IDisposable
    {
        private readonly Dictionary<int, EntryFile> _hashToFileMap = new();

        [Title("ID", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private string _buildId;
        [SerializeField]
        private int _levelId;

        [Title("Data", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private float _time;
        [SerializeField]
        private float _gameTime;

        [TableList]
        [SerializeField]
        [ReadOnly]
        private EntryFile[] _entries;

        [TableList]
        [SerializeField]
        [ReadOnly]
        private List<EntryFile> _fileList;

        private float _lastFlushTime = 0;
        private DateTime _lastExitTime;

        public int LevelId { get => _levelId; set => _levelId = value; }
        public float Time { get => _time; set => _time = value; } 
        public float GameTime { get => _gameTime + (_time - _lastFlushTime); }
        public DateTime LastExitTime { get => _lastExitTime; }

        

        public void Initialize(float time)
        {
            if (_entries.IsNullOrEmpty())
            {
                _fileList = new List<EntryFile>();
            }
            else
            {
                _fileList = new List<EntryFile>(_entries);
            }

            var map = _hashToFileMap;
            var span = _fileList.AsReadOnlySpanUnsafe();

            for (int i = 0; i < span.Length; i++)
            {
                var hash = span[i].Hash;
                if(map.TryAdd(hash, span[i]))
                {

                }
            }
            _time = time;
            _lastFlushTime = time;
        }

        public void Dispose()
        {
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(string uniqueName, out T data) where T : IFile, new()
            => TryGet<T>(uniqueName.GetHashCode(), out data);

        public bool TryGet<T>(int hash, out T data) where T : IFile, new()
        {
            var result = _hashToFileMap.TryGetValue(hash, out var entry);

            if (result == false)
            {
                var file = new T();
                entry = new EntryFile(hash, file);

                _fileList.Add(entry);
                _hashToFileMap[hash] = entry;
            }

            if (entry.File is T weakData)
            {
                data = weakData;
                return true;
            }

            data = default(T);
            return false;
        }

    }
}
