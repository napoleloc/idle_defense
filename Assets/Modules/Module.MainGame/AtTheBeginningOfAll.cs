using System.Threading;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.AddressableKeys;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Module.GameUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module.MainGame
{
    public partial class AtTheBeginningOfAll : MonoBehaviour
    {
        // Scenes
        private const string SCENE_GAMEPLAY_NAME = "1-scene-gameplay";
        private const string SCENE_ENTITIES_ENEMY_NAME = "2-scene-entities-enemy";
        private const string SCENE_DEBUG_NAME = "4-scene-debug";

        // Addressable keys
        private const string WORLD_CAMERA_NAME = "prefab-world-camera";
        private const string WORLD_TIMER_NAME = "prefab-world-timer";
        private const string WORLD_AUDIO_NAME = "prefab-world-audio";

        private const string GAMEPLAY_MANAGER_NAME = "prefab-gameplay-manager";
        private const string CURRENCY_MANAGER_NAME = "prefab-currency-manager";
        private const string INPUT_RECEVIER_NAME = "prefab-input-recevier";
        private const string MAP_LOADER_NAME = "prefab-map-loader";

        private const string WORLD_ENEMY_NAME = "prefab-world-enemy";

        private CancellationTokenSource _initCts;

        private async void Start()
        {
            RenewInitCts();
            await InitGameAsync(_initCts.Token);
        }

        private async UniTask InitGameAsync(CancellationToken token)
        {
            await UniTask.NextFrame();
            await WorldMessenger.Publisher.UIScope()
                .PublishAsync(new AsyncMessage<ShowScreenMessage>(ScreenNames.MainLobbyScreenName().ToShowScreenMessage()));

            await InitMainSceneAsyncInternal(token);
            await InitGamePlaySceneAsyncInternal(token);
            await InitEnemySceneAsyncInternal(token);
            await InitDebugSceneAsyncInternal(token);
        }

        private async UniTask InitMainSceneAsyncInternal(CancellationToken token)
        {
            var mainScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            var cameraHandle = new AddressableKey<GameObject>(WORLD_CAMERA_NAME);
            var timerHandle = new AddressableKey<GameObject>(WORLD_TIMER_NAME);
            var audioHandle = new AddressableKey<GameObject>(WORLD_AUDIO_NAME);
          
            await cameraHandle.InstantiateAsync(mainScene, trimCloneSuffix: true);
            await timerHandle.InstantiateAsync(mainScene, trimCloneSuffix: true);
            await audioHandle.InstantiateAsync(mainScene, trimCloneSuffix: true);
         
        }

        private async UniTask InitGamePlaySceneAsyncInternal(CancellationToken token)
        {
            await SceneManager.LoadSceneAsync(SCENE_GAMEPLAY_NAME, LoadSceneMode.Additive);
            var gamePlayScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            var handleGameplay = new AddressableKey<GameObject>(GAMEPLAY_MANAGER_NAME);
            var currencyHandle = new AddressableKey<GameObject>(CURRENCY_MANAGER_NAME);
            var handleInput = new AddressableKey<GameObject>(INPUT_RECEVIER_NAME);
            var handleMapLoader = new AddressableKey<GameObject>(MAP_LOADER_NAME);
            
            await handleGameplay.InstantiateAsync(gamePlayScene, trimCloneSuffix: true);
            await currencyHandle.InstantiateAsync(gamePlayScene, trimCloneSuffix: true);
            await handleInput.InstantiateAsync(gamePlayScene, trimCloneSuffix: true);
            await handleMapLoader.InstantiateAsync(gamePlayScene, trimCloneSuffix: true);
        }

        private async UniTask InitEnemySceneAsyncInternal(CancellationToken token)
        {
            //TODO: Initialization logic for the created scene.
            var scene = SceneManager.CreateScene(SCENE_ENTITIES_ENEMY_NAME);

            var handleWorldEnemy = new AddressableKey<GameObject>(WORLD_ENEMY_NAME);

            await handleWorldEnemy.InstantiateAsync(scene, trimCloneSuffix: true);
        }

        private async UniTask InitDebugSceneAsyncInternal(CancellationToken token)
        {
            await SceneManager.LoadSceneAsync(SCENE_DEBUG_NAME, LoadSceneMode.Additive);
        }

        private void RenewInitCts()
        {
            _initCts ??= new();
            if (_initCts.IsCancellationRequested)
            {
                _initCts.Dispose();
                _initCts = new();
            }
        }
    }
}
