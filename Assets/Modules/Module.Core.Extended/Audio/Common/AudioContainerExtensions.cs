using System;
using EncosyTower.Modules.Vaults;
using EncosyTower.Modules;
using JetBrains.Annotations;

namespace Module.Core.Extended.Audio
{
    public static class AudioContainerExtensions
    {
        public static bool TryAddToGlobalObjectVaultAs<T>(
            [NotNull] this AudioContainer container
            , AudioContainerId id
            , ref T result
        )
            where T : AudioContainer
        {
            if(string.Equals(container.name, id.name, StringComparison.Ordinal) == false
                || container is not T containerT)
            {
                return false;
            }

            GlobalObjectVault.TryAdd(new PresetId(id.id), containerT);
            result = containerT;
            return true;
        }
    }
}
