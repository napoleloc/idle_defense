using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame.Attribute.Control
{
    public class AttributeControl : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMP_Text _labelValue;
        [SerializeField]
        private Button _controlButton;

        public void InitializeComponent()
        {

        }

        public void Cleanup()
        {

        }
    }
}
