using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules;
using EncosyTower.Modules.Data;
using EncosyTower.Modules.Logging;
using UnityEngine;

namespace Module.Data.MasterDatabase
{
    public class WorldMasterDatabase
    {
        private const string RESOURCE_PATH = "Database/DatabaseAsset";

        private static DatabaseAsset s_database;

        public static DatabaseAsset Database
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (s_database == false)
                {
                    var handle = new ResourceKey<DatabaseAsset>(RESOURCE_PATH);
                    s_database = handle.Load();

                    if (s_database)
                    {
                        DevLoggerAPI.LogInfo("[WorldMasterDatabase] DatabaseAsset loaded successfully.");
                        s_database.Initialize();
                    }
                    else
                    {
                        DevLoggerAPI.LogError("[WorldMasterDatabase] DatabaseAsset not found at path: 'Resources/Database/DatabaseAsset'. " +
                                   "Ensure the asset exists and is in the correct Resources folder.");
                    }
                }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetDataTableAsset<T>([NotNull] string name, out T dataTable) where T : DataTableAsset
            => Database.TryGetDataTableAsset<T>(name, out dataTable);
    }
}
