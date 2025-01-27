using EncosyTower.Modules.AtlasedSprites;
using Module.Entities.Tower.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class TowerConfigAsset : ScriptableObject
    {
        [SerializeField]
        private TowerIdConfig _id;

        [Title("Addressables Keys", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] 
        private string _sprite;
        [SerializeField] 
        private string _atlased;

        public TowerIdConfig Id => _id;
        public AtlasedSpriteKey AlasedSpriteKey => new(_sprite, _atlased);
    }
}
