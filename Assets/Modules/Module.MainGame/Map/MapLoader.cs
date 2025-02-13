using System.Threading;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.PubSub;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.PubSub;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Module.MainGame.Map
{
    public class MapLoader : MonoBehaviour
    {
        private const string SCENE_MAP_FORMAT = "1-scene-gameplay-map-{0}-{0}";

        public static readonly Id<MapLoader> PresetId = default;

        [SerializeReference]
        [BoxGroup("Debugging", centerLabel: true)]
        [SerializeField]
        private ushort _mapId;
        [BoxGroup("Debugging", centerLabel: true)]
        [SerializeField]
        private ushort _region;

        private Option<SceneInstance> _option;

        private void Awake()
        {
            var subscriber = WorldMessenger.Subscriber.MapScope().WithState(this);
            subscriber.Subscribe<AsyncMessage<LoadMapMessage>>(static (state, msg) => state.HandleAsync(msg));
            subscriber.Subscribe<AsyncMessage<UnloadMapMessage>>(static (state, msg) => state.HandleAsync(msg));

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        public void InitData(ushort mapId, ushort region)
        {
            _mapId = mapId;
            _region = region;
        }

        [Button(buttonSize: 35, parameterBtnStyle: ButtonStyle.FoldoutButton, Icon = SdfIconType.Download)]
        public async UniTask LoadAsync()
           => await LoadAsycnInternal();

        [Button(buttonSize: 35, parameterBtnStyle: ButtonStyle.FoldoutButton, Icon = SdfIconType.Trash)]
        public async UniTask UnloadAsync()
            => await UnloadAsyncInternal();

        private async UniTask HandleAsync(AsyncMessage<LoadMapMessage> asyncMessage)
            => await LoadAsycnInternal();

        private async UniTask HandleAsync(AsyncMessage<UnloadMapMessage> asyncMessage)
            => await UnloadAsyncInternal();

        private async UniTask LoadAsycnInternal(CancellationToken token = default)
        {
            if (_option.HasValue)
            {
                await UnloadAsyncInternal();
            }

            var handleMap = new AddressableKey<Scene>(string.Format(SCENE_MAP_FORMAT, _mapId, _region));
            _option = await handleMap.TryLoadAsync(LoadSceneMode.Additive, token: token);
        }

        private async UniTask UnloadAsyncInternal()
        {
            if (_option.HasValue)
            {
                await Addressables.UnloadSceneAsync(_option.Value());
            }
        }
    }
}
