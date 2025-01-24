using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Collections;

namespace Module.Data.Runtime.Serialization
{
    public class RuntimeDataSerializeContainer
    {
        public FasterList<RuntimeDataSerialize> entries = new();

        public ReadOnlyMemory<RuntimeDataSerialize> Entries
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => entries.AsReadOnlyMemory();
        }

        public void AddEntry(RuntimeDataSerialize entry)
            => entries.Add(entry);
    }
}
