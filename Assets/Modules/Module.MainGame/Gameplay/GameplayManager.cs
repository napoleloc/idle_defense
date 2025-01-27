using System.Threading;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Logging;
using EncosyTower.Modules.PubSub;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Module.Entities.Tower;
using Module.Entities.Tower.PubSub;
using Module.GameCommon.PubSub;
using Module.GameUI;
using Module.MainGame.Map;
using UnityEngine;

namespace Module.MainGame.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        private bool _isPaused;

        private void Awake()
        {
            var subscriber = WorldMessenger.Subscriber.Global().WithState(this);

            subscriber.Subscribe<AsyncMessage<StartGameMessage>>(static (state, msg) => state.HandleAsync(msg));
            subscriber.Subscribe<AsyncMessage<QuitGameMessage>>(static (state, msg) => state.HandleAsync(msg));

            subscriber.Subscribe<PauseGameMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<UnpauseGameMessage>(static (state, msg) => state.Handle(msg));
        }

        private void OnApplicationFocus(bool focus)
        {
#if !UNITY_EDITOR
            _isPaused = !hasFocus;
            SaveOnPaused();
#endif
        }

        private void OnApplicationPause(bool pauseStatus)
        {
#if !UNITY_EDITOR
            _isPaused = pauseStatus;
            SaveOnPaused();
#endif
        }

        private void SaveOnPaused()
        {
            if (_isPaused)
            {
                
            }
        }

        private async UniTask HandleAsync(AsyncMessage<StartGameMessage> asyncMessage)
            => await StartGameAsyncInternal();

        private async UniTask HandleAsync(AsyncMessage<QuitGameMessage> asyncMessage)
            => await QuitGameAsyncInternal();

        private void Handle(PauseGameMessage message)
        {
            DevLoggerAPI.LogInfo("Pause Game!");
        }

        private void Handle(UnpauseGameMessage message)
        {
            DevLoggerAPI.LogInfo("Unpause Game!");
        }

        private async UniTask StartGameAsyncInternal(CancellationToken token = default)
        {
            // TODO:
            await GlobalValueVault<bool>.WaitUntil(MapLoader.PresetId, true, token);
            await GlobalValueVault<bool>.WaitUntil(BuildingSpawner.PresetId, true, token);

            // TODO:
            await WorldMessenger.Publisher.MapScope()
                .PublishAsync(new AsyncMessage<LoadMapMessage>(new LoadMapMessage()));

            await WorldMessenger.Publisher.TowerScope()
                .PublishAsync(new AsyncMessage<LoadTowerMessage>(new LoadTowerMessage(1)));

            // TODO:
            await WorldMessenger.Publisher.UIScope()
                .PublishAsync(new AsyncMessage<ShowScreenMessage>(ScreenNames.InGameScreenName().ToShowScreenMessage()));
        }

        private async UniTask QuitGameAsyncInternal(CancellationToken token = default)
        {
            await WorldMessenger.Publisher.MapScope()
                .PublishAsync(new AsyncMessage<UnloadMapMessage>(new UnloadMapMessage()));

            WorldMessenger.Publisher.TowerScope()
                .Publish(new UnloadMapMessage());
        }
    }
}
