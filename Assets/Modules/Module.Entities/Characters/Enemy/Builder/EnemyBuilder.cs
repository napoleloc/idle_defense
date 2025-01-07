using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Logging;
using EncosyTower.Modules.Pooling;
using UnityEngine;

namespace Module.Entities.Characters.Enemy.Builder
{
    public abstract class EnemyBuilder<TId> : MonoBehaviour
        where TId : unmanaged
    {
        protected readonly Dictionary<TId, GameObjectPool> PoolMap = new();
        
        protected abstract UniTask InitializePool();

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

        public void ReturnToPool(TId id, GameObject instance)
        {
            if (PoolMap.TryGetValue(id, out var pool) == false)
            {
                DevLoggerAPI.LogError("in valid!");
                return;
            }

            pool.Return(instance);
        }

        public void Provide(EnemyBuildingOptions options)
        {

        }

    }
}
