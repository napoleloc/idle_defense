using System;
using System.Runtime.CompilerServices;
using EncosyTower.Modules.Logging;
using Module.GameCommon.Save;
using Module.GameCommon.Save.Multiplatforms;
using UnityEngine;

namespace Module.Data.GameSave
{
    public static class GameSaveManager
    {
        private const string SAVE_FILE_NAME = "save";
        private const int SAVE_DELAY = 30;

        private static bool s_isSaveLoaded;
        private static bool s_isSaveRequired;
        private static string s_json;
        private static FileTable s_fileTable;

        public static bool IsSaveLoaded => s_isSaveLoaded;
        public static int LevelId { get => s_fileTable.LevelId; set => s_fileTable.LevelId = value; }
        public static float GameTime => s_fileTable.GameTime;
        public static DateTime LastExitTime => s_fileTable.LastExitTime;

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_json = string.Empty;
            s_fileTable = null;
        }
#endif

        public static void Initialize(
            bool useAutoSave = false
            , bool clearSave = false
            , float overrideTime = -1.0F)
        {
            if (clearSave) 
                InitializeAndClearInternal(overrideTime != -1f ? overrideTime : Time.time);
            else
                Load(overrideTime != -1F ? overrideTime : Time.time);

            if (useAutoSave)
            {

            }
        }

        public static void Deinitialize()
        {
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SaveData()
            => Save();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateTime(float time)
            => s_fileTable.Time = time;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteSaveFile()
            => BaseSaveWrapper.ActiveWrapper.Delete(SAVE_FILE_NAME);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>(int hash, out T data) where T : IFile, new()
        {
            if(s_isSaveLoaded == false)
            {
                DevLoggerAPI.LogError("[GameSaveManager]:  has not been initialized!");
                data = default(T);
                return false;
            }

            return s_fileTable.TryGet(hash, out data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet<T>(string uniqueName, out T data) where T : IFile, new()
        {
            if (s_isSaveLoaded == false)
            {
                DevLoggerAPI.LogError("[GameSaveManager]:  has not been initialized!");
                data = default(T);
                return false;
            }

            return s_fileTable.TryGet(uniqueName, out data);
        }

        private static void InitializeAndClearInternal(float time)
        {
            s_fileTable = new();
            s_fileTable.Initialize(time);

            DevLoggerAPI.LogInfo("[GameSaveManager]: Created clear save!");

            s_isSaveLoaded = true;
        }

        private static void Load(float time)
        {
            s_fileTable = BaseSaveWrapper.ActiveWrapper.Load(SAVE_FILE_NAME);
            s_fileTable.Initialize(time);

            s_isSaveLoaded = true;

            DevLoggerAPI.LogInfo("[Game Save Manager]: Save is loaded!");
        }

        private static void Save(bool forceSave = false)
        {
            if(forceSave == false && s_isSaveLoaded == false)
            {
                return;
            }

            if(s_fileTable == null)
            {
                return;
            }

            //s_fileTable.Flush()

            BaseSaveWrapper.ActiveWrapper.Save(s_fileTable, SAVE_FILE_NAME);
            s_isSaveLoaded = true;

            DevLoggerAPI.LogInfo("[GameSaveManager]: Game is saved!");
        }
        
    }
}
