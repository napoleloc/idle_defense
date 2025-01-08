using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Logging;
using EncosyTower.Modules.Pooling;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module.Entities.Characters.Enemy.Builder
{
    public abstract class EnemyBuilder<TId>
        where TId : unmanaged
    {
        protected readonly Dictionary<TId, GameObjectPool> PoolMap = new();

        public abstract UniTask InitializePool(Scene scene);

        public bool TryGetFromPool(TId id, bool activate, out GameObject instance)
        {
            if (PoolMap.TryGetValue(id, out GameObjectPool pool))
            {
                instance = pool.RentGameObject(activate);
                return true;
            }

            instance = default(GameObject);
            return false;
        }

        public GameObject GetFromPool(TId id, bool activate)
        {
            if (PoolMap.TryGetValue(id, out GameObjectPool pool))
            {
                return pool.RentGameObject(activate);
            }

           return default(GameObject);
        }

        public void ReturnToPool(TId id, GameObject instance)
        {
            if (PoolMap.TryGetValue(id, out var pool) == false)
            {
                DevLoggerAPI.LogError("in valid!");
                return;
            }

            pool.Return(instance);
        }
    }
}
