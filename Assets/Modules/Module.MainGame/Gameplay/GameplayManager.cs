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
using Module.Worlds.BattleWorld.Map;
using Module.Worlds.BattleWorld.Map.PubSub;
using UnityEngine;

namespace Module.MainGame.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        private void Awake()
        {
            var subscriber = WorldMessenger.Subscriber.Global().WithState(this);

            subscriber.Subscribe<AsyncMessage<StartGameMessage>>(static (state, msg) => state.HandleAsync(msg));
            subscriber.Subscribe<AsyncMessage<QuitGameMessage>>(static (state, msg) => state.HandleAsync(msg));

            subscriber.Subscribe<PauseGameMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<UnpauseGameMessage>(static (state, msg) => state.Handle(msg));
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
            await GlobalValueVault<bool>.WaitUntil(TowerLoader.PresetId, true, token);

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
