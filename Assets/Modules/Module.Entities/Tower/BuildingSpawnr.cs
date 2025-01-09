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
    public class BuildingSpawnr : MonoBehaviour
    {
        public static readonly Id<BuildingSpawnr> PresetId = default;

        [SerializeField]
        private TowerDatabase _database;    

        private GameObject _usedObject;
        private bool _loaded;

        public TowerDatabase Database
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _database;
        }

        private void Awake()
        {
            var subscriber = WorldMessenger.Subscriber.TowerScope().WithState(this);
            subscriber.Subscribe<AsyncMessage<LoadTowerMessage>>(static (state, msg) => state.HandleAsync(msg));
            subscriber.Subscribe<LoadTowerMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<UnloadTowerMessage>(static (state, msg) => state.Handle(msg));

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private async UniTask HandleAsync(AsyncMessage<LoadTowerMessage> asyncMessage)
        {
            var message = asyncMessage.Message;
            await LoadAsyncInternal(message.Id);
        }

        private void Handle(LoadTowerMessage message)
        {

        }

        private void Handle(UnloadTowerMessage message)
        {

        }

        private async UniTaskVoid LoadAndForget(ushort id)
            => await LoadAsyncInternal(id);

        private async UniTask LoadAsyncInternal(ushort id)
        {
            if (_loaded)
            {
                UnloadInternal();
            }

            var towerHandle = new AddressableKey<GameObject>(TowerNames.Format(id));

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
