using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Logging;
using UnityEngine;

namespace Module.Data.Runtime
{
    public abstract class DataTable<TDataId, TData> : DataTableBase<TDataId, TData>
    {
        private readonly Dictionary<TDataId, int> _idToIndexMap = new();

        protected Dictionary<TDataId, int> IdToIndexMap
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _idToIndexMap;
        }

        protected internal override void Initialize()
        {
            var map = _idToIndexMap;
            var entries = Entries.Span;

            map.Clear();
            map.EnsureCapacity(entries.Length);

            for ( var i = 0; i < entries.Length; i++ )
            {
                var id = GetId(entries[i]);

                if (map.TryAdd(id, i) == false)
                {
                    ErrorDuplicateId(id, i, this);
                    continue;
                }
            }
        }

        public virtual bool TryGetEntry(TDataId id, out TData entry)
        {
            var result = _idToIndexMap.TryGetValue(id, out var index);
            entry = result ? Entries.Span[index] : default;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual string ToString(TDataId value)
           => value.ToString();

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        protected static void ErrorDuplicateId(
              TDataId id
            , int index
            , [NotNull] DataTable<TDataId, TData> context
        )
        {
            DevLoggerAPI.LogErrorFormat(
                  context
                , "Id with value \"{0}\" is duplicated at row \"{1}\""
                , context.ToString(id)
                , index
            );
        }
    }
}
