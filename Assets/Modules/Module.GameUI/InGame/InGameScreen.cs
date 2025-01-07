using System;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame
{
    public class InGameScreen : NavScreen
    {
        [BoxGroup("Buttons", centerLabel: true)]
        [SerializeField]
        private Button _pauseButton;

        public override UniTask Initialize(Memory<object> args)
        {
            _pauseButton.onClick.AddListener(PauseButton_OnClick);

            return base.Initialize(args);
        }

        public override UniTask Cleanup(Memory<object> args)
        {
            _pauseButton.onClick.RemoveAllListeners();

            return base.Cleanup(args);
        }

        private void PauseButton_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ModalNames.PauseModalName().ToShowModalMessage());
        }
    }
}
