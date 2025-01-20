using System;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Module.GameUI.Talents;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.MainLobby.Talents
{
    public class TalentsScreen : NavScreen
    {
        [SerializeField]
        private Button _buttonClose;

        [SerializeField]
        private TalentPanel _talentPanel;

        public override UniTask Initialize(Memory<object> args)
        {
            _buttonClose.onClick.AddListener(ButtonClose_OnClick);

            return base.Initialize(args);
        }

        public override UniTask Cleanup(Memory<object> args)
        {
            _buttonClose.onClick.RemoveAllListeners();

            return base.Cleanup(args);
        }

        private void ButtonClose_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(new HideScreenMessage(false));
        }
    }
}
