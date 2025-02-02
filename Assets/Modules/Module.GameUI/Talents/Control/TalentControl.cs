using Module.Data.Runtime.DataTableAsstes.Talents;
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
        private AttributeKind _kind;
        [SerializeField]
        [ReadOnly]
        private AttributeType _attribute;

        public void Initialize(RuntimeTalentIdData id)
        {
            _kind = id.Kind;
            _attribute = id.Type;

            _buttonControl.onClick.AddListener(ButtonControl_OnClick);
        }

        public void SetAllLabels(string name, string value, string cost)
        {
            _labeTalentName.SetText(name);
            _labelTalentValue.SetText(value);
            _labelCostAmount.SetText(cost);
        }

        public void SetAllImages(Sprite spriteTalent, Sprite spriteCost)
        {
            _imageTalent.sprite = spriteTalent;
            _imageCost.sprite = spriteCost;
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
