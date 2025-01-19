using Module.GameUI.Talents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.GameUI.InGame
{
    public class HUDPanel : MonoBehaviour
    {
        [Title("Panels", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private HealthBarUI _healthBar;
        [SerializeField]
        private TalentPanel _attributeGroup;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private uint _currentIndexPage;

        public void InitializeComponent()
        {
            _healthBar.InitializeComponent();
        }

        public void Cleanup()
        {
            _healthBar.Cleanup();
        }
    }
}
