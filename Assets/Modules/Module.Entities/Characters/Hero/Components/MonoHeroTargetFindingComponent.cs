using Module.Entities.Characters.Enemy.AI;
using Module.Entities.Characters.Enemy;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules.Vaults;
using EncosyTower.Modules.Logging;

namespace Module.Entities.Characters.Hero
{
    public class MonoHeroTargetFindingComponent : MonoBehaviour
    {
        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private bool _hasTarget;
        [SerializeField]
        private float _searchRange = 5.5F;
        [SerializeField]
        private float _searchInerval = 1;
        [SerializeField, ReadOnly]
        private float _nextClosestCheckTime;

        private WorldEnemy _worldEnemy;
        private EnemyAIController _enemyTarget;

        public bool HasTarget => _hasTarget;
        public EnemyAIController TargetEnemy => _enemyTarget;

        public void Initialize()
        {
            GlobalObjectVault.TryGet(WorldEnemy.PresetId, out _worldEnemy);
        }

        public void UpdateComponent()
        {
            if (Time.time >= _nextClosestCheckTime + _searchInerval)
            {
                FindClosestTargetInternal();
                _nextClosestCheckTime = Time.time;
            }
        }

        private void FindClosestTargetInternal()
        {
            if (_hasTarget == false)
            {
                var enemies = _worldEnemy.GetEnemies();
                _hasTarget = ValidateClosestTarget(enemies.AsReadOnlySpan());
            }
            else
            {
                float currentDistance = Vector3.Distance(Vector3.zero, _enemyTarget.transform.position);
                if (currentDistance > _searchRange || _enemyTarget.gameObject.activeSelf == false)
                {
                    _enemyTarget = default(EnemyAIController);
                    _hasTarget = false;
                }
            }
        }

        private bool ValidateClosestTarget(ReadOnlySpan<EnemyAIController> enemies)
        {
            bool flag = false;
            int lenght = enemies.Length;
            int closestIndex = 0;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < lenght; i++)
            {
                var enemy = enemies[i];

                float currentDistance = Vector3.Distance(Vector3.zero, enemy.transform.position);

                if (currentDistance < closestDistance && currentDistance < _searchRange)
                {
                    flag = true;
                    closestIndex = i;
                    closestDistance = currentDistance;
                }
            }

            _enemyTarget = flag ? enemies[closestIndex] : default;
            return flag;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, _searchRange);
        }
    }
}
