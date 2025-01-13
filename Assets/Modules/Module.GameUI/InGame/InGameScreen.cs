using System;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.PubSub;
using Module.Core.Extended.UI;
using Module.Entities.Characters.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame
{
    public class InGameScreen : NavScreen
    {
        [Title("Panels", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private InGameProgressPanel _progressPanel;
        [SerializeField]
        private HUDPanel _hudPanel;

        [Title("Buttons", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private Button _pauseButton;
        [SerializeField]
        private Button _startButton;

        public override UniTask Initialize(Memory<object> args)
        {
            _progressPanel.InitializeComponent();
            _hudPanel.InitializeComponent();

            _pauseButton.onClick.AddListener(ButtonPause_OnClick);
            _startButton.onClick.AddListener(ButtonStart_OnClick);

            return base.Initialize(args);
        }

        public override UniTask Cleanup(Memory<object> args)
        {
            _progressPanel.Cleanup();
            _hudPanel.Cleanup();

            _pauseButton.onClick.RemoveAllListeners();
            _startButton.onClick.RemoveAllListeners();

            return base.Cleanup(args);
        }

        private void ButtonPause_OnClick()
        {
            WorldMessenger.Publisher.UIScope()
                .Publish(ModalNames.PauseModalName().ToShowModalMessage());
        }

        private void ButtonStart_OnClick()
        {
            GlobalObjectVault.TryGet(EnemyProgressManager.PresetId, out var manager);
            manager.BeginProgress();
        }
    }
}
