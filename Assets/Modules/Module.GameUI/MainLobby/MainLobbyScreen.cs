using System;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Module.GameCommon.PubSub;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.MainLobby
{
    public class MainLobbyScreen : NavScreen
    {
        [Title("Button Events", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] 
        private Button _buttonMail;
        [SerializeField] 
        private Button _buttonSetting;
        [SerializeField] 
        private Button _buttonShop;
        [SerializeField] 
        private Button _buttonAbility;
        [SerializeField] 
        private Button _buttonTalents;
        [SerializeField] 
        private Button _buttonQuickFight;

        public override UniTask Initialize(Memory<object> args)
        {
            _buttonMail.onClick.AddListener(ButtonMail_OnClick);
            _buttonSetting.onClick.AddListener(ButtonSetting_OnClick);

            _buttonShop.onClick.AddListener(ButtonShop_OnClick);
            _buttonAbility.onClick.AddListener(ButtonAbility_OnClick);
            _buttonTalents.onClick.AddListener(ButtonTalents_OnClick);
            _buttonQuickFight.onClick.AddListener(ButtonQuickFight_OnClick);

            return base.Initialize(args);
        }

        public override UniTask Cleanup(Memory<object> args)
        {
            _buttonMail.onClick.RemoveAllListeners();
            _buttonSetting.onClick.RemoveAllListeners();

            _buttonShop.onClick.RemoveAllListeners();
            _buttonAbility.onClick.RemoveAllListeners();
            _buttonTalents.onClick.RemoveAllListeners();
            _buttonQuickFight.onClick.RemoveAllListeners();

            return base.Cleanup(args);
        }

        private void ButtonMail_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ModalNames.MailModalName().ToShowModalMessage());
        }

        private void ButtonSetting_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ModalNames.SettingsModalName().ToShowModalMessage());
        }

        private void ButtonShop_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ScreenNames.ShopScreenName().ToShowScreenMessage());
        }

        private void ButtonAbility_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ScreenNames.AbilityScreenName().ToShowScreenMessage());
        }

        private void ButtonTalents_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ScreenNames.TalentsScreenName().ToShowScreenMessage());
        }

        private void ButtonQuickFight_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(new HideScreenMessage());

            WorldMessenger.Publisher.Global()
               .PublishAsync(new AsyncMessage<StartGameMessage>(new StartGameMessage()));
        }
    }
}
