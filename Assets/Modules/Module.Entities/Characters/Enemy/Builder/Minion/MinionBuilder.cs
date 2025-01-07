using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.Pooling;
using JetBrains.Annotations;
using Module.GameCommon;
using UnityEngine;

namespace Module.Entities.Characters.Enemy.Builder
{
    public class MinionBuilder : EnemyBuilder<MinionId>
    {
        public static MinionBuilder CreateInstance([NotNull] BuildingConfig buildingConfig, Transform parent)
        {
            var type = TypeCache.Get<MinionBuilder>();
            var go = new GameObject(buildingConfig.Name, type);
            go.transform.parent = parent;

            return go.GetComponent<MinionBuilder>();
        }

        protected override async UniTask InitializePool()
        {
            var map = PoolMap;

            var ids = MinionIdExtensions.Values.AsMemory();
            var idsLenght = ids.Length;

            for ( var i = 0; i < idsLenght; i++)
            {
                var handleSource = new AddressableKey<GameObject>(EnemyNames.MinionName(ids.Span[i]));
                var sourceOpt = await handleSource.TryLoadAsync();

                if(sourceOpt.HasValue == false)
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

