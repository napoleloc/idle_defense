using System.Runtime.CompilerServices;
using Module.Core.Pooling;
using Module.GameUI.Talents.Control;
using UnityEngine;

namespace Module.GameUI.Talents
{
    public class TalentControlPooler : MonoBehaviour
    {
        [SerializeField]
        private Transform _parent;
        [SerializeField]
        private GameObject _source;

        private ComponentPool<ComponentPrefab, TalentControl> _pool;

        public ComponentPool<ComponentPrefab, TalentControl> Pool
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _pool;
        }

        public void Initialize(bool prepoolOnStart, int prepoolAmount)
        {
            _pool = new(new() {
                Source = _source,
                Parent = _parent,
            });

            if (prepoolOnStart)
            {
                PrepoolOnStart(prepoolAmount);
            }
        }

        private void PrepoolOnStart(int prepoolAmount)
            => _pool.Prepool(prepoolAmount);
    }
}
