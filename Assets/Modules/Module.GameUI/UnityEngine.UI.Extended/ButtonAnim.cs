using EncosyTower.Modules;
using LitMotion;
using LitMotion.Extensions;
using Module.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Module.GameUI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class ButtonAnim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const string ANIM_H = "Anim";
        private const string ANIM_IN = "Anim/In";
        private const string ANIM_OUT = "Anim/Out";

        [OnValueChanged("OnValueChanged")]
        [SerializeField]
        private bool _interactable;

        [HorizontalGroup(ANIM_H, order: 1, Width = 0.5F)]
        [TabGroup(ANIM_IN, "In", SdfIconType.AlarmFill, TextColor = "#FFE60B", Order = 1)]
        [LabelText("Duration", SdfIconType.HourglassSplit)]
        [PropertyRange(0, 1)]
        [EnableIf("_interactable")]
        [SerializeField]
        private float _durationIn;

        [TabGroup(ANIM_IN, "In", SdfIconType.AlarmFill, TextColor = "#FFE60B", Order = 1)]
        [LabelText("Easing", SdfIconType.HourglassSplit)]
        [EnableIf("_interactable")]
        [SerializeField]
        private Ease _easingIn;

        [TabGroup(ANIM_OUT, "Out", SdfIconType.AlarmFill, TextColor = "#00FFFF", Order = 2)]
        [LabelText("Duration", SdfIconType.HourglassSplit)]
        [PropertyRange(0, 1)]
        [EnableIf("_interactable")]
        [SerializeField]
        private float _durationOut;

        [TabGroup(ANIM_OUT, "Out", SdfIconType.AlarmFill, TextColor = "#00FFFF", Order = 2)]
        [LabelText("Easing", SdfIconType.HourglassSplit)]
        [EnableIf("_interactable")]
        [SerializeField] 
        private Ease _easingOut;

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private MotionHandle _handleScale;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public bool Interactable
        {
            get => _interactable;
            set
            {
                if (_canvasGroup.IsInvalid())
                {
                    _canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
                }

                _interactable = value;
                _canvasGroup.interactable = _interactable;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_interactable == false)
            {
                return;
            }

            _handleScale.TryComplete();
         
            _handleScale = LMotion.Create(1, 0.8F, _durationOut)
                .WithEase(_easingIn)
                .Bind((value) => _rectTransform.localScale = Vector3.one * value);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if( _interactable == false)
            {
                return;
            }

            _handleScale.TryCancel();

            _handleScale = LMotion.Create(0.8F, 1.0F, _durationOut)
                .WithEase(_easingOut)
                .Bind((value) => _rectTransform.localScale = Vector3.one * value);
        }

#if UNITY_EDITOR
        private void OnValueChanged()
        {
            Interactable = _interactable;
        }
#endif
    }
}
