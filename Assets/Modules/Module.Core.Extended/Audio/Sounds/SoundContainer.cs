using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Collections.Unsafe;
using EncosyTower.Modules.Vaults;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Audio.Sounds
{
    public class SoundContainer : AudioContainer, ISoundContainer
    {
        public static readonly Id<SoundContainer> PresetId = default;

        private readonly FasterList<SoundPool> _pausedSounds = new();
        private readonly List<SoundPool> _usedObjects = new();

        public int UsedCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _usedObjects.Count;
        }

        public static SoundContainer CreateInstance([NotNull] AudioContainerConfig config, Transform parent)
        {
            var type = TypeCache.Get<SoundContainer>();
            var go = new GameObject(config.ContainerName, type);
            go.transform.parent = parent;

            return go.GetOrAddComponent<SoundContainer>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            InitializePool<SoundPool>("prefab-sound-pool", transform);
            
            GlobalObjectVault.TryAdd(PresetId, this);
        }

        protected override void Deinitialize()
        {
            base.Deinitialize();

            _usedObjects.Clear();

            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PlaySound()
            => PlaySoundAndForget().Forget();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask PlaySoundAsync()
            => await PlaySoundAsyncInternal();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseSound()
            => PauseSoundInternal();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnpauseSound()
            => UnpauseSoundInternal();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopSound()
            => StopSoundInternal();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReturnToPool(SoundPool soundPool)
            => ReturnToPoolInternal(soundPool);

        private async UniTask PlaySoundAndForget()
            => await PlaySoundAsync();

        private async UniTask PlaySoundAsyncInternal()
        {
            var soundPool = GetFromPool<SoundPool>();

            _usedObjects.Add(soundPool);

            var audioClip = await GetAudioClipAsyncInternal("sfx_ui_claim", initCts.Token);
            soundPool.Initialize(1, false, audioClip, default);

            await soundPool.PlaySoundAsync();
        }

        private void PauseSoundInternal()
        {
            var usedObjects = _usedObjects.AsSpanUnsafe();
            var lenght = usedObjects.Length;

            _pausedSounds.Clear();
            _pausedSounds.IncreaseCapacityTo(lenght);
            _pausedSounds.CopyTo(usedObjects);

            for (var i = 0; i < lenght; i++)
            {
                usedObjects[i].PauseSound();
            }
        }

        private void UnpauseSoundInternal()
        {
            var pausedSounds = _pausedSounds.AsSpan();
            var lenght = pausedSounds.Length;

            for (var i = 0; i <= lenght; i++)
            {
                pausedSounds[i].UnpauseSound();
            }
        }

        private void StopSoundInternal()
        {
            var usedObjects = _usedObjects.AsSpanUnsafe();
            var lenght = usedObjects.Length;

            for (var i = 0; i < lenght; i++)
            {
                usedObjects[i].StopSound();
            }
        }

        private void ReturnToPoolInternal(SoundPool soundPool)
        {
            if (soundPool == false)
            {
                return;
            }

            _usedObjects.Remove(soundPool);
            ReturnToPool(soundPool.gameObject);
        }
    }
}
