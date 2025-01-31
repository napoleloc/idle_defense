using EncosyTower.Modules.Data;
using UnityEngine;

namespace Module.Data.MasterDatabase.Talents
{
    public partial struct TalentMultiplierData : IData
    {
        [SerializeField]
        private ushort _id;
        [SerializeField]
        private int _cost;
        [SerializeField]
        private float _multiplier;
    }
}
