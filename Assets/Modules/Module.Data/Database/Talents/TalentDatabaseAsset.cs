using System.Threading;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.AtlasedSprites;
using Module.Worlds.BattleWorld.Attribute;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Data.Database.Talents
{
    [CreateAssetMenu(fileName =nameof(TalentDatabaseAsset))]
    public class TalentDatabaseAsset : ScriptableObject
    {
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "_attribute")]
        [SerializeField]
        private TalentData[] _talents;

        [System.Serializable]
        private class TalentData
        {
            [SerializeField]
            private AttributeType _attribute;
            [SerializeField]
            private uint _unlockCost;

            [Title("Addressables Keys", titleAlignment: TitleAlignments.Centered)]
            [SerializeField]
            private string _talentAtlasedKey;
            [SerializeField]
            private string _talentSpriteKey;

            public async UniTask<Sprite> GetSpriteTalentAsync(CancellationToken token = default)
            => await GetSpriteTalentSpriteAsyncInternal(token);

            private async UniTask<Sprite> GetSpriteTalentSpriteAsyncInternal(CancellationToken token)
            {
                var atlasedSpriteKey = new AtlasedSpriteKey(_talentAtlasedKey, _talentSpriteKey);
                var handle = new AtlasedSpriteKeyAddressables(atlasedSpriteKey);

                var sprite = await handle.LoadAsync(token);

                return sprite.IsValid() ? sprite : default;
            }
        }
    }
}
