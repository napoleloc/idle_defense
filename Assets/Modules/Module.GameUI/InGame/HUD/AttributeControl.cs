using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame
{
    public class AttributeControl : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _labelAttribute;
        [SerializeField]
        private Image _iconAttribute;

        [SerializeField]
        private Button _upgradeButton;

        public void InitializeComponent()
        {
            _upgradeButton.onClick.AddListener(UpgradeButton_OnClick);
        }

        public void Cleanup()
        {
            _upgradeButton.onClick.RemoveAllListeners();
        }

        private void UpgradeButton_OnClick()
        {

        }
    }
}
