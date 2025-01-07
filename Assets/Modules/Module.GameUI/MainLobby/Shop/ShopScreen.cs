using System;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.MainLobby.Shop
{
    public class ShopScreen : NavScreen
    {
        [SerializeField] private Button _closeButton;

        [SerializeField] private Button _specialPacksButton;
        [SerializeField] private Button _chapterPacksButton;
        [SerializeField] private Button _equipmentPacksButton;
        [SerializeField] private Button _currenciesPacksButton;

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

        private void SpecialPacksButton_OnClick()
        {

        }

        private void ChapterPacksButton_OnClick()
        {

        }

        private void EquipmentPacksButton_OnClick()
        {

        }

        private void CurrencyPacksButton_OnClick()
        {

        }
    }
}
