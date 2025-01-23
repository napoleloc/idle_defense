using System;
using Module.GameUI.Talents.GridSheet;
using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Module.GameUI.Talents
{
    public class TalentPanel : MonoBehaviour, IDisposable
    {
        [Title("Direct Reference", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private TalentControlGridSheet _talentControlGridSheet;
        [SerializeField]
        private Button[] _activateSheetButtons;

        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private AttributeKind _currentSheetIndex;

        public void OnAwake()
        {
            _talentControlGridSheet.TableData.Initialize();
        }

        public void Dispose()
        {
            _talentControlGridSheet.TableData.Deinitialize();
        }

        public void Initialize()
        {
            _talentControlGridSheet.Initialize();

            var buttons = _activateSheetButtons.AsSpan();
            for (int i = 0; i < buttons.Length; i++)
            {
                var attributeKind = (AttributeKind)i;
                buttons[i].onClick.AddListener(() => ButtonActivateSheet_OnClick(attributeKind));
            }
        }

        private void ButtonActivateSheet_OnClick(AttributeKind attributeKind)
        {
            _currentSheetIndex = attributeKind;
            _talentControlGridSheet.OnChangeAttributeKind(attributeKind);
        }
    }
}
