using Cysharp.Threading.Tasks;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.Pooling;
using Module.GameCommon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module.Entities.Characters.Enemy.Builder
{
    public class MinionBuilder : EnemyBuilder<MinionId>
    {
        public override async UniTask InitializePool(Scene scene)
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

