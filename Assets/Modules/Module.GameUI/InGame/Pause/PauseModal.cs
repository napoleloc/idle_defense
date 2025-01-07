using System;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Module.GameCommon.PubSub;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame.Pause
{
    public class PauseModal : NavModal
    {
        [BoxGroup("Buttons", centerLabel: true)]
        [SerializeField] 
        private Button _backToHomeButton;
        [BoxGroup("Buttons", centerLabel: true)]
        [SerializeField] 
        private Button _relayButton;
        [BoxGroup("Buttons", centerLabel: true)]
        [SerializeField] 
        private Button _unpauseButton;

        public override UniTask Initialize(Memory<object> args)
        {
            _unpauseButton.onClick.AddListener(UnpauseButton_OnClick);

            return base.Initialize(args);
        }

        public override UniTask Cleanup(Memory<object> args)
        {
            _unpauseButton.onClick.RemoveAllListeners();

            return base.Cleanup(args);
        }

        public override void DidPushEnter(Memory<object> args)
            => WorldMessenger.Publisher.Global().Publish(new PauseGameMessage());

        public override void DidPopExit(Memory<object> args)
            => WorldMessenger.Publisher.Global().Publish(new UnpauseGameMessage());

        private void BackToHomeButton_OnClick()
        {
            WorldMessenger.Publisher.Global()
                .PublishAsync(new AsyncMessage<QuitGameMessage>(new QuitGameMessage()));
        }

        private void RelayButton_OnClick()
        {

        }

        private void UnpauseButton_OnClick()
            => WorldMessenger.Publisher.UIScope().Publish(new HideModalMessage());
    }
}
