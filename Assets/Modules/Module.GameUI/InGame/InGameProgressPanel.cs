using EncosyTower.Modules.PubSub;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Module.GameUI
{
    public class InGameProgressPanel : MonoBehaviour
    {
        [Title("Components", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private RectTransform _rectTransform;
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [Title("Elements", titleAlignment: TitleAlignments.Centered)]
        [SerializeField]
        private TMP_Text _labelTime;

        private ISubscription _subscription;

        public void Initialize()
        {

        }

        public void Cleanup()
        {
            _subscription?.Unsubscribe();
        }
    }
}
