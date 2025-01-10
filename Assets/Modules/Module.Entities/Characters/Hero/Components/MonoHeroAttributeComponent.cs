using UnityEngine;

namespace Module.Entities.Characters.Hero
{
    public class MonoHeroAttributeComponent : MonoBehaviour
    {
        [SerializeField]
        private float _normalAttackInterval;
        [SerializeField]
        private float _specialAttackInterval;

        public float NormalAttackInterval => _normalAttackInterval;
        public float SpecialAttackInterval => _specialAttackInterval;
    }
}
