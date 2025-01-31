using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.Talents.Control
{
    public class TalentControl : MonoBehaviour
    {
        [SerializeField]
        private Image _imageTalent;
        [SerializeField]
        private Image _imageCost;
        [SerializeField]
        private TMP_Text _labeTalentName;
        [SerializeField]
        private TMP_Text _labelTalentValue;
        [SerializeField]
        private TMP_Text _labelCostAmount;
        [SerializeField]
        private Button _buttonControl;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        [ReadOnly]
        private AttributeType _attributeType;

        public void Initialize(AttributeType attributeType)
        {
            _attributeType = attributeType;
            _labeTalentName.SetText(AttributeTypeExtensions.ToStringFast(attributeType));

            _buttonControl.onClick.AddListener(ButtonControl_OnClick);
        }

        public void Cleanup()
        {
            _buttonControl.onClick.RemoveAllListeners();
        }

        private void ButtonControl_OnClick()
        {

        }
    }
}
