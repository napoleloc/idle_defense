using Cysharp.Threading.Tasks;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.Pooling;
using Module.GameCommon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module.Entities.Characters.Enemy.Builder
{
    public class EliteBuilder : EnemyBuilder<EliteId>
    {
        public override async UniTask InitializePool(Scene scene)
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
                    Scene = scene,
                };

                if (map.TryAdd(ids.Span[i], new GameObjectPool(prefab)))
                {
                    continue;
                }
            }
        }
    }
}
