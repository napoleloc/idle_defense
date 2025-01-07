using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.Pooling;
using JetBrains.Annotations;
using Module.GameCommon;
using UnityEngine;

namespace Module.Entities.Characters.Enemy.Builder
{
    public class EliteBuilder : EnemyBuilder<EliteId>
    {
        public static EliteBuilder CreateInstance([NotNull] BuildingConfig buildingConfig, Transform parent)
        {
            var type = TypeCache.Get<EliteBuilder>();
            var go = new GameObject(buildingConfig.Name, type);
            go.transform.parent = parent;

            return go.GetComponent<EliteBuilder>();
        }

        public override async UniTask InitializePool()
        {
            var map = PoolMap;

            var ids = EliteIdExtensions.Values.AsMemory();
            var idsLenght = ids.Length;

            for (var i = 0; i < idsLenght; i++)
            {
                var handleSource = new AddressableKey<GameObject>(EnemyNames.EliteName(ids.Span[i]));
                var sourceOpt = await handleSource.TryLoadAsync();

                if (sourceOpt.HasValue == false)
                {
                    continue;
                }

                var prefab = new GameObjectPrefab(){
                    Source = sourceOpt.ValueOrDefault(),
                    Scene = gameObject.scene,
                };

                if (map.TryAdd(ids.Span[i], new GameObjectPool(prefab)))
                {
                    continue;
                }
            }
        }
    }
}
