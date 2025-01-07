using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using UnityEngine;

namespace Module.Worlds.BattleWorld.Quest
{
    public class QuestProgressManager : MonoBehaviour
    {
        public readonly static Id<QuestProgressManager> PresetId = default;

        private void Awake()
        {
            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }
    }
}
