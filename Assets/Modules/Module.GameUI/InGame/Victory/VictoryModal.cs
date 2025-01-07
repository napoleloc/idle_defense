using Module.Core.Extended.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame.Victory
{
    public class VictoryModal : NavModal
    {
        [SerializeField]
        private TMP_Text _labelReward;
        [SerializeField]
        private TMP_Text _labelDoubleReward;

        [BoxGroup("Buttons", centerLabel: true)]
        [SerializeField] 
        private Button _backToHomeButton;
        [BoxGroup("Buttons", centerLabel: true)]
        [SerializeField] 
        private Button _doubleRewardButton;

        protected override void Start()
        {
            _backToHomeButton.onClick.AddListener(BackToHomeButton_OnClick);
            _doubleRewardButton.onClick.AddListener(DoubleRewardButton_OnClick);
        }

        private void BackToHomeButton_OnClick()
        {

        }

        private void DoubleRewardButton_OnClick()
        {

        }
    }
}
