using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.InGame
{
    public class AttributeControl : MonoBehaviour
    {
        [SerializeField]  private Image _imageAttribute;
        [SerializeField]  private TMP_Text _labelAttribute;
        [SerializeField]  private Button _buttonModifierAttribute;

        public void InitializeComponent()
        {
            
        }

        public void Cleanup()
        {
           
        }

        private void ButtonModifierAttribute_OnClick()
        {

        }
    }
}
