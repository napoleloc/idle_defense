using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Pooling;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Module.Core.Extended.Audio
{
    public class AudioContainer : MonoBehaviour, IAudioContainer
    {
        private readonly Dictionary<string, AudioClip> _nameToClipMap = new();
        private readonly Dictionary<int, AsyncOperationHandle<AudioClip>> _hashCodeToHandleMap = new();

        protected GameObjectPool pool;
        protected CancellationTokenSource initCts;

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Deinitialize();
        }

        protected virtual void Initialize()
        {
            RenewInitCts();
        }

        protected virtual void Deinitialize()
        {

            initCts.Cancel();
            pool.ReleaseInstances(0);

            _nameToClipMap.Clear();
            _hashCodeToHandleMap.Clear();
        }

        protected virtual void InitializePool<T>(string sourceName, Transform parent) where T : AudioPool
        {
            var type = TypeCache.Get<T>();
            var source = new GameObject(sourceName, type);
            source.transform.parent = parent;
            source.SetActive(false);

            var prefab = new GameObjectPrefab()
            {
                Source = source,
                Parent = transform,
            };

            pool = new GameObjectPool(prefab);
        }
       
        protected T GetFromPool<T>() where T : AudioPool
        {
            var go = pool.RentGameObject(true);
            if(go.TryGetComponent<T>(out T typelessAudio))
            {
                if(typelessAudio is T typedAudio)
                {
                    return typedAudio;
                }
            }

            return default(T);
        }

        protected void ReturnToPool(GameObject instance)
        {
            if(instance == false)
            {
                return;
            }

            pool.Return(instance);
        }

        protected void ReturnToPool<T>(T audio) where T : AudioPool
        {
            if(audio == false)
            {
                return;
            }

            pool.Return(audio.gameObject);
        }

        internal protected async UniTask<AudioClip> GetAudioClipAsyncInternal(string addressAudioClip, CancellationToken token = default)
        {
            var map = _nameToClipMap;

            if(map.TryGetValue(addressAudioClip, out var audioClip) == false)
            {
                audioClip = await LoadAudioClipAsyncInternal(addressAudioClip, token);
                map[addressAudioClip] = audioClip;
            }

            return audioClip;
        }

        internal protected void ReleaseInternal()
        {
            var handles = _hashCodeToHandleMap.Values;
            foreach(var handle in handles)
            {
                Addressables.Release(handle);
            }

            _hashCodeToHandleMap.Clear();
        }

        private async UniTask<AudioClip> LoadAudioClipAsyncInternal(string addressAudioClip, CancellationToken token = default)
        {
            var map = _hashCodeToHandleMap;

            var handle = Addressables.LoadAssetAsync<AudioClip>(addressAudioClip);
          
            if (handle.IsValid() == false)
            {
                return default;
            }

            var hashCode = addressAudioClip.GetHashCode();
            map.TryAdd(hashCode, handle);

            while (handle.IsDone == false)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                await UniTask.NextFrame(token);

                if (token.IsCancellationRequested)
                {
                    break;
                }
            }

            if (token.IsCancellationRequested)
            {
                return default;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                return default;
            }

            var asset = handle.Result;

            return (asset is UnityEngine.AudioClip audioClip && audioClip) || asset != null
                ? asset : default;
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
