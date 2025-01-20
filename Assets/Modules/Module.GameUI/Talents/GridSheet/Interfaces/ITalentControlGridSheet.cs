using Module.Data.GameSave.Talents;

namespace Module.GameUI.Talents.GridSheet
{
    public interface ITalentControlGridSheet
    {
        void Initialize(TalentTableData tableData);
        void Cleanup();
    }
}
