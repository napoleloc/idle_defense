using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules;
using EncosyTower.Modules.Collections;
using UnityEngine.AddressableAssets;

namespace Module.Core.Extended.Audio
{
    public interface IAudioLoaderAsset { }

    [CreateAssetMenu]
    public class AudioLoaderAsset : ScriptableObject, IAudioLoaderAsset
    {
        [SerializeField]
        private AudioType[] _preloadTypes;
        [SerializeField]
        private AudioType[] _types;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [ShowInInspector]
        [ReadOnly]
        private FasterList<AudioClip> _audioClips = new();

        private readonly Dictionary<AudioType, int> _typeToIndex = new Dictionary<AudioType, int>();

        public ReadOnlyMemory<AudioType> PreloadTypes
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _preloadTypes;
        }

        public ReadOnlyMemory<AudioType> Types
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _types;
        }

        public ReadOnlyMemory<AudioClip> AudioClips
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioClips.AsReadOnlyMemory();
        }

        public async UniTask InitializeAsync(CancellationToken token = default)
        {
            await PreloadAsync(token);
        }

        public void Deinitialize()
        {
            foreach (var audioCl in AudioClips.Span)
            {
                Addressables.Release(audioCl);
            }

            _audioClips.Clear();
            _typeToIndex.Clear();
        }

        public async UniTask PreloadAsync(CancellationToken token = default)
        {
            var typeToIndex = _typeToIndex;
            var audioClips = _audioClips;
            var preloadTypes = PreloadTypes;

            for (var i = 0; i < preloadTypes.Length; i++)
            {
                var type = preloadTypes.Span[i];
                var audioOtp = await LoadAudioClipAsyncInternal(AudioClipNames.AudioNameBy(type), token);
                if (audioOtp.HasValue)
                {
                    audioClips.Add(audioOtp.Value());
                    typeToIndex.TryAdd(type, i);
                }
            }
        }

        public bool TryGetClip(AudioType type, out AudioClip clip)
        {
            var result = _typeToIndex.TryGetValue(type, out var index);
            clip = result ? AudioClips.Span[index] : null;
            return result;
        }

        private async UniTask<Option<AudioClip>> LoadAudioClipAsyncInternal([NotNull] string key, CancellationToken token = default)
        {
            var handle = new AddressableKey<AudioClip>(key);

            var audioOtp = await handle.TryLoadAsync(token);
            return audioOtp;
        }
    }
}
