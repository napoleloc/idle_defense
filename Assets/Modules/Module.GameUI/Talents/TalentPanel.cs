using System;
using System.Threading;
using Module.GameUI.Talents.GridSheet;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.Talents
{
    public class TalentPanel : MonoBehaviour
    {
        [SerializeField]
        private TalentControlGridSheet[] _talentControlGridSheets;
        [SerializeField]
        private Button[] _activateSheetButtons;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private int _currentSheetIndex;
        [SerializeField, ReadOnly]
        private int _previourSheetIndex;

        public void Initialize(CancellationToken token)
        {
            var sheets = _talentControlGridSheets.AsSpan();
            for (int i = 0; i < sheets.Length; i++)
            {
                sheets[i].Initialize();
            }

            var buttons = _activateSheetButtons.AsSpan();
            for (int i = 0; i < buttons.Length; i++)
            {
                var index = i;
                buttons[index].onClick.AddListener(() => ButtonActivateSheet_OnClick(index));
            }
        }

        private void ButtonActivateSheet_OnClick(int indexSheet)
        {
            if(_currentSheetIndex == indexSheet)
            {
                return;
            }

            _previourSheetIndex = _currentSheetIndex;
            _currentSheetIndex = indexSheet;

            var sheets = _talentControlGridSheets.AsSpan();
            for (int i = 0;i < sheets.Length; i++)
            {
                var sheet = sheets[i];
            }
        }
    }
}
