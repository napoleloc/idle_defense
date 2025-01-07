using EncosyTower.Modules.AtlasedSprites;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class TowerConfigAsset : ScriptableObject
    {
        [Title("Addressables Keys", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] 
        private string _sprite;
        [SerializeField] 
        private string _atlased;

        public string Sprite => _sprite;
        public string Atlased => _atlased;
        public AtlasedSpriteKey AlasedSpriteKey => new(_sprite, _atlased);
    }
}
