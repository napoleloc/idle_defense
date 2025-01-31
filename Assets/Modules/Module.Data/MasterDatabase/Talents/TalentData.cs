using System;
using EncosyTower.Modules.Data;

namespace Module.Data.MasterDatabase.Talents
{
    public partial class TalentData : IData
    {
        [DataProperty]
        public TalentIdData Id => Get_Id();

        [DataProperty]
        public float Value => Get_Value();

        [DataProperty]
        public ReadOnlyMemory<TalentMultiplierData> Multipliers => Get_Multipliers();
    }
}
