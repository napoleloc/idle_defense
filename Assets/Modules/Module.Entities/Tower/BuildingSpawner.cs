using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.PubSub;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.PubSub;
using Module.Entities.Tower.Data;
using Module.Entities.Tower.PubSub;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Module.Entities.Tower
{
    public class BuildingSpawner : MonoBehaviour
    {
        public static readonly Id<BuildingSpawner> PresetId = default;

        [SerializeField]
        private TowerDatabaseAsset _database;    

        private GameObject _usedObject;
        private bool _loaded;

        public TowerDatabaseAsset Database
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _database;
        }

        private void Awake()
        {
            var subscriber = WorldMessenger.Subscriber.TowerScope().WithState(this);
            subscriber.Subscribe<AsyncMessage<SpawnTowerMessage>>(static (state, msg) => state.HandleAsync(msg));
            subscriber.Subscribe<SpawnTowerMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<ReleaseTowerMessage>(static (state, msg) => state.Handle(msg));

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private async UniTask HandleAsync(AsyncMessage<SpawnTowerMessage> asyncMessage)
        {
            var message = asyncMessage.Message;
            await LoadAsyncInternal(message.Id);
        }

        private void Handle(SpawnTowerMessage message)
        {

        }

        private void Handle(ReleaseTowerMessage message)
        {

        }

        private async UniTaskVoid LoadAndForget(TowerIdConfig id)
            => await LoadAsyncInternal(id);

        private async UniTask LoadAsyncInternal(TowerIdConfig id)
        {
            if (_loaded)
            {
                UnloadInternal();
            }

            var towerHandle = new AddressableKey<GameObject>(TowerNames.Format(id.Kind, id.Type));

            _usedObject = await towerHandle.InstantiateAsync(gameObject.scene, trimCloneSuffix: true);
        }

        private void UnloadInternal()
        {
            if (_usedObject.IsValid())
            {
                Addressables.ReleaseInstance(_usedObject);
            }
        }
    }
}
