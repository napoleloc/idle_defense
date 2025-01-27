using Module.GameCommon;
using UnityEngine;

namespace Module.Entities.Tower.Data
{
    [System.Serializable]
    public struct TowerIdConfig
    {
        [SerializeField]
        private TowerKind _kind;
        [SerializeField]
        private TowerType _type;

        public TowerKind Kind => _kind;
        public TowerType Type => _type;
    }
}
