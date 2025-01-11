using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame
{
    public class HUDPanel : MonoBehaviour
    {
        [Title("Panels", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private HealthBarUI _healthBar;
        [SerializeField]
        private AttributesPanel[] _panels;

        [Title("Buttons", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private Button[] _activatePageButtons;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private uint _currentIndexPage;

        public void InitializeComponent()
        {
            _healthBar.InitializeComponent();

            var panels = _panels.AsSpan();
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].Initialize();
            }

            var buttons = _activatePageButtons.AsSpan();
            for (int i = 0; i < buttons.Length; i++)
            {
                var indexPage = (uint)(i);
                buttons[i].onClick.AddListener(() => ActivatePageButton_OnClick(indexPage));
            }
        }

        public void Cleanup()
        {
            _healthBar.Cleanup();

            var panels = _panels.AsSpan();
            for (var i = 0; i < panels.Length; i++)
            {
                panels[i].Cleanup();
            }

            var buttons = _activatePageButtons.AsSpan();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.RemoveAllListeners();
            }
        }

        private void ActivatePageButton_OnClick(uint indexPage)
        {
            if (_currentIndexPage == indexPage)
            {
                return;
            }
            _currentIndexPage = indexPage;
        }

        private void InitalizePageInternal()
        {

        }
    }
}
