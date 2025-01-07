using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Vaults;
using JetBrains.Annotations;
using UnityEngine;

namespace Module.Core.Extended.Audio.Musics
{
    public class MusicContainer : AudioContainer, IMusicContainer
    {
        public static readonly Id<MusicContainer> PresetId = default;

        private readonly FasterList<MusicPool> _usedObjects = new();

        public static MusicContainer CreateInstance([NotNull] AudioContainerConfig config, Transform parent)
        {
            var type = TypeCache.Get<MusicContainer>();
            var go = new GameObject(config.ContainerName, type);
            go.transform.parent = parent;

            return go.GetComponent<MusicContainer>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            InitializePool<MusicPool>("prefab-music-pool", transform);

            GlobalObjectVault.TryAdd(PresetId, this);
        }

        protected override void Deinitialize()
        {
            base.Deinitialize();

            _usedObjects.Clear();

            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        public void PlayMusic()
        {
            PlayMusicAndForget().Forget();
        }

        public async UniTask PlayMusicAsync()
        {
           await PlayMusicAsyncInternal();
        }

        public void PauseMusic()
        {
             
        }

        public void PauseAllMusic()
        {

        }

        public void UnpauseMusic()
        {

        }

        public void StopMusic()
        {

        }

        public void ReturnToPool(MusicPool musicPool)
        {
            if(musicPool == false)
            {
                return;
            }

            _usedObjects.Remove(musicPool);
            ReturnToPool(musicPool.gameObject);
        }

        public void StopAllMusic()
        {

        }

        private async UniTaskVoid PlayMusicAndForget()
        {
            await PlayMusicAsyncInternal();
        }

        private async UniTask PlayMusicAsyncInternal()
        {
            var musicPool = GetFromPool<MusicPool>();
            _usedObjects.Add(musicPool);

            await musicPool.PlayMusicAsync();
        }

        
    }
}
