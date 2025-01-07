using Cysharp.Text;
using LitMotion;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Module.Core.Extended.Currency.UI
{
    public class CurrencyPanel : MonoBehaviour
    {
        private const string ERROR_DURATION = "Duration must be <= 0.";

        [EnumToggleButtons]
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;
        [SerializeField] private Button _addButton;

        [FoldoutGroup("Animation")]
        [LabelText(SdfIconType.CameraVideo)]
        [SerializeField] 
        private bool _playAnimation;

        [FoldoutGroup("Animation")]
        [EnableIf("_playAnimation")]
        [ValidateInput("@_duration > 0", ERROR_DURATION)]
        [LabelText(SdfIconType.HourglassSplit)]
        [PropertyRange(0, 2)]
        [SerializeField]
        private float _duration;

        [FoldoutGroup("Animation")]
        [EnableIf("@_playAnimation && _duration > 0")]
        [LabelText(SdfIconType.AlarmFill)]
        [SerializeField] 
        private Ease _easing;

        private MotionHandle _handle;
        private int _value;

        public CurrencyType CurrencyType => _currencyType;

        public void Initialize()
        {
            CurrencyManager.TryReigster(_currencyType, OnChangedCurrency);
        }

        public void Cleanup()
        {
            CurrencyManager.TryUnregister(CurrencyType, OnChangedCurrency);
        }

        private void AddButton_OnClick()
        {
        }

        private void OnChangedCurrency(int currency)
        {
            int fromValue = CurrencyExntensions.GetAmountFromData(CurrencyType.Coins) + currency;

            if (_playAnimation)
            {
                _handle.TryComplete();
                _handle = LMotion.Create(_value, fromValue, _duration)
                    .WithEase(_easing)
                    .Bind((value) =>{
                        using(var stringBuilder = new Utf16ValueStringBuilder())
                        {
                            stringBuilder.Append(value);
                            _text.SetText(stringBuilder);
                        }
                    });
            }
            else
            {
                using(var stringBuilder = new Utf16ValueStringBuilder())
                {
                    stringBuilder.Advance(fromValue);
                    _text.SetText(stringBuilder);
                }
            }
        }
    }
}
