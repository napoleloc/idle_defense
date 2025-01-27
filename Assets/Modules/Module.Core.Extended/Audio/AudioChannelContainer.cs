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

        protected ComponentPool<ComponentPrefab, T> pool;
        protected CancellationTokenSource initCts;

        public bool Initialized { get; private set; }
        public AudioLoaderAsset LoaderAsset => _loaderAsset;
        internal ComponentPool<ComponentPrefab, T> Pool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => pool;
        }

        public async UniTask InitializeAsync()
        {
            if (Initialized)
            {
                return;
            }

            pool = new(new() {
                Source = _source,
                Parent = _parent,
            });

            RenewInitCts();
            await _loaderAsset.InitializeAsync(initCts.Token);

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
            initCts ??= new();
            if (initCts.IsCancellationRequested)
            {
                initCts.Dispose();
                initCts = new();
            }
        }
    }
}
