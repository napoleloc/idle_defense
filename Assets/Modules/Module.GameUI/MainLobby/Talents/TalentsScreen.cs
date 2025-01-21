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
        private TalentPool _pool;
        [SerializeField]
        private TalentPanel _talentPanel;

        protected override void Awake()
        {
            _pool.Initialize(5);
            _talentPanel.OnAwake();
        }

        protected override void OnDestroy()
        {
            _pool.Deinitialize();
            _talentPanel.Dispose();
        }

        public override UniTask Initialize(Memory<object> args)
        {
            _buttonClose.onClick.AddListener(ButtonClose_OnClick);

            _talentPanel.Initialize();

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
