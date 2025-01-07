using System;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Module.GameCommon.PubSub;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.MainLobby
{
    public class MainLobbyScreen : NavScreen
    {
        [SerializeField] private Button _mailButton;
        [SerializeField] private Button _settingsButton;

        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _quickFightButton;

        public override UniTask Initialize(Memory<object> args)
        {
            _mailButton.onClick.AddListener(MailButton_OnClick);
            _settingsButton.onClick.AddListener(SettingsButton_OnClick);

            _shopButton.onClick.AddListener(ShopButton_OnClick);
            _inventoryButton.onClick.AddListener(InventoryButton_OnClick);
            _quickFightButton.onClick.AddListener(QuickFightButton_OnClick);

            return base.Initialize(args);
        }

        public override UniTask Cleanup(Memory<object> args)
        {
            _mailButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();

            _shopButton.onClick.RemoveAllListeners();
            _inventoryButton.onClick.RemoveAllListeners();
            _quickFightButton.onClick.RemoveAllListeners();

            return base.Cleanup(args);
        }

        private void MailButton_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ModalNames.MailModalName().ToShowModalMessage());
        }

        private void SettingsButton_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ModalNames.SettingsModalName().ToShowModalMessage());
        }

        private void ShopButton_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ScreenNames.ShopScreenName().ToShowScreenMessage());
        }

        private void InventoryButton_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ScreenNames.InventoryScreenName().ToShowScreenMessage());
        }

        private void QuickFightButton_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(new HideScreenMessage());

            WorldMessenger.Publisher.Global()
               .PublishAsync(new AsyncMessage<StartGameMessage>(new StartGameMessage()));
        }
    }
}
