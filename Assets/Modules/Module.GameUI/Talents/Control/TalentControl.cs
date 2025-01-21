using System;
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

        private event Action ButtonControl_OnClick;

        public void Initialize(AttributeType attributeType)
        {
            _attributeType = attributeType;

            _labeTalentName.SetText(AttributeTypeExtensions.ToDisplayStringFast(attributeType));
        }

        public void Cleanup()
        {
            _buttonControl.onClick.RemoveAllListeners();
        }
    }
}
