using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.Logging;
using Module.Core.Pooling;
using Module.Entities.Characters.Enemy.AI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module.Entities.Characters.Enemy.Pooling
{
    public abstract class EnemyPoolManager<TId, TComponent> : MonoBehaviour
        where TId : unmanaged
        where TComponent : EnemyAIController
    {
        private readonly Dictionary<TId, ComponentPool<ComponentPrefab, TComponent>> _idToPool = new();

        public Scene Scene { get; set; }

        public abstract UniTask InitializeAsync();

        /// <summary>
        /// Preloads a resource associated with the given ID and adds it to a pool for later use.
        /// </summary>
        /// <typeparam name="TId">The type of the ID used to identify the resource.</typeparam>
        /// <param name="id">The ID of the resource to preload.</param>
        /// <param name="scene">The scene where the resource will be used.</param>
        /// <returns>A UniTask representing the asynchronous operation.</returns>
        protected async UniTask PreloadAndPoolAsync(TId id, Scene scene)
        {
            var idToPool = _idToPool;
            var handle = new AddressableKey<GameObject>(EnemyNames.GetNameFromId(id));
            var sourceOpt = await handle.TryLoadAsync();

            if (sourceOpt.HasValue == false)
            {
                return;
            }

            var prefab = new ComponentPrefab(){
                Source = sourceOpt.ValueOrDefault(),
                Scene = scene,
            };

            if (idToPool.TryAdd(id, new ComponentPool<ComponentPrefab, TComponent>(prefab)) == false)
            {
                DevLoggerAPI.LogWarning($"Failed to add ID '{id}' to the pool. The ID might already exist or the pool is full.");
            }
        }

        public void Release()
        {
            foreach (var pool in _idToPool.Values)
            {
                pool.ReleaseInstances(0);
            }

            _idToPool.Clear();
        }

        public TComponent Rent(TId id, bool activate = false)
        {
            if (_idToPool.TryGetValue(id, out var pool) == false)
            {
                LogErrorCannotFindPool(id, this);
                return null;
            }
            
            return pool.RentComponent(activate);
        }

        public void Return(TId id, TComponent instance)
        {
            if (_idToPool.TryGetValue(id, out var pool) == false)
            {
                LogErrorCannotFindPool(id, this);
                return;
            }

            pool.Return(instance);
        }

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorCannotFindPool(TId id, EnemyPoolManager<TId, TComponent> context)
        {
            DevLoggerAPI.LogError(context, $"Cannot find any pool id {id}.");
        }
    }
}
