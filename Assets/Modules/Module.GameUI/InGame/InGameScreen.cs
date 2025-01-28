using System;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Module.Entities.Characters.Enemy;
using Module.GameUI.Talents;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame
{
    public class InGameScreen : NavScreen
    {
        private const string TITLE_DIRECT_REF = "Direct Reference";
        private const string TITLE_DIRECT_BUTTON_REF = "Direct Reference/Buttons";

        [BoxGroup(TITLE_DIRECT_REF, centerLabel: true)]
        [SerializeField]
        private TalentControlPooler _pooler;
        [BoxGroup(TITLE_DIRECT_REF, centerLabel: true)]
        [SerializeField]
        private TalentPanel _talentPanel;

        [BoxGroup(TITLE_DIRECT_BUTTON_REF, centerLabel: true)]
        [SerializeField]
        private Button _buttonPause;
        [BoxGroup(TITLE_DIRECT_BUTTON_REF, centerLabel: true)]
        [SerializeField]
        private Button _buttonStart;

        protected override void Awake()
        {
            _pooler.Initialize(true, 5);
        }

        protected override void OnDestroy()
        {
            _pooler.Deinitialize();
        }

        public override UniTask Initialize(Memory<object> args)
        {
            _talentPanel.Initialize();

            _buttonPause.onClick.AddListener(ButtonPause_OnClick);
            _buttonStart.onClick.AddListener(ButtonStart_OnClick);

            return base.Initialize(args);
        }

        public override UniTask Cleanup(Memory<object> args)
        {
            _talentPanel.Cleanup();

            _buttonPause.onClick.RemoveAllListeners();
            _buttonStart.onClick.RemoveAllListeners();

            return base.Cleanup(args);
        }

        private void ButtonPause_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ModalNames.PauseModalName().ToShowModalMessage());
        }

        private void ButtonStart_OnClick()
        {
        }
    }
}
