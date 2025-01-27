using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Module.Core.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Audio
{
    public abstract class AudioChannelContainer<T> : MonoBehaviour, IAudioChannelContainer
        where T : AudioChannel
    {
        [Title("Hard Reference", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private AudioLoaderAsset _loaderAsset;
        [SerializeField]
        private Transform _parent;
        [SerializeField]
        private GameObject _source;

        private readonly Dictionary<AudioType, T> _typeToChannel = new();
        private ComponentPool<ComponentPrefab, T> _pool;
        private CancellationTokenSource _initCts;

        public bool Initialized { get; private set; }
        public AudioLoaderAsset LoaderAsset => _loaderAsset;
        
        protected Dictionary<AudioType, T> TypeToChannel
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _typeToChannel;
        }

        protected ComponentPool<ComponentPrefab, T> Pool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _pool;
        }

        internal protected virtual T GetFromPool(AudioType audioType)
        {
            var component = _pool.RentComponent(true);
            var id = component.GetInstanceID();

            _typeToChannel[audioType] = component;

            return component;
        }

        internal protected virtual void ReturnToPool(T instance, Action listener)
        {
            _pool.Return(instance);
            _typeToChannel.Remove(instance.AudioType);

            listener?.Invoke();
        }

        public async UniTask InitializeAsync()
        {
            if (Initialized)
            {
                return;
            }

            _pool = new(new() {
                Source = _source,
                Parent = _parent,
            });

            RenewInitCts();
            await _loaderAsset.InitializeAsync(_initCts.Token);

            Initialized = true;
        }

        public void Deinitialize()
        {
            if(Initialized == false)
            {
                return;
            }

            Initialized = false;

            _loaderAsset.Deinitialize();
        }

        private void RenewInitCts()
        {
            _initCts ??= new();
            if (_initCts.IsCancellationRequested)
            {
                _initCts.Dispose();
                _initCts = new();
            }
        }
    }
}
