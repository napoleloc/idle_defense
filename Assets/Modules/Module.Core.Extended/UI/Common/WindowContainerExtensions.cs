using EncosyTower.Modules.Vaults;
using EncosyTower.Modules;
using System;
using ZBase.UnityScreenNavigator.Core.Windows;
using System.Diagnostics.CodeAnalysis;

namespace Module.Core.Extended.UI
{
    public static class WindowContainerExtensions
    {
        public static bool TryAddToGlobalObjectVaultAs<T>(
              [NotNull] this WindowContainerBase container
            , WindowContainerId id
            , ref T result
        )
            where T : WindowContainerBase
        {
            if (string.Equals(container.name, id.name, StringComparison.Ordinal) == false
                || container is not T containerT
            )
            {
                return false;
            }

            GlobalObjectVault.TryAdd(new PresetId(id.id), containerT);
            result = containerT;
            return true;
        }
    }
}
