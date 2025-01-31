using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.Data;
using EncosyTower.Modules.Logging;
using UnityEngine;

namespace Module.Data.MasterDatabase
{
    public class WorldMasterDatabase
    {
        private const string KEY = nameof(DatabaseAsset);

        private static DatabaseAsset s_database;

        public static DatabaseAsset Database
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (s_database == false)
                {
                    var handle = new AddressableKey<DatabaseAsset>(KEY);
                    s_database = handle.Load();
                }

                DevLoggerAPI.LogInfo("[DatabaseManager] DatabaseAsset loaded successfully.");

                return s_database;
            }
        }

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_database?.Deinitialize();
            s_database = null;
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetDataTableAsset([NotNull] string name, out DataTableAsset dataTable)
            => Database.TryGetDataTableAsset(name, out dataTable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetDataTableAsset([NotNull] Type type, out DataTableAsset dataTable)
            => Database.TryGetDataTableAsset(type, out dataTable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetDataTableAsset<T>(out T dataTable) where T : DataTableAsset
            => Database.TryGetDataTableAsset<T>(out dataTable);
    }
}
