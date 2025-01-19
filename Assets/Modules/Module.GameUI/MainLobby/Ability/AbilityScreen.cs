using System;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.MainLobby.Ability
{
    public class AbilityScreen : NavScreen
    {
        [SerializeField] private Button _closeButton;

        public override UniTask Initialize(Memory<object> args)
        {
            _closeButton.onClick.AddListener(CloseButton_OnClick);

            return base.Initialize(args);
        }

        public override UniTask Cleanup(Memory<object> args)
        {
            _closeButton.onClick.RemoveAllListeners();

            return base.Cleanup(args);
        }

        private void CloseButton_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
               .Publish(new HideScreenMessage(false));
        }
    }
}
