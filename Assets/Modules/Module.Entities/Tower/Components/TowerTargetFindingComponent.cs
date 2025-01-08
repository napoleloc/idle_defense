using System;
using Module.Entities.Characters.Enemy;
using Module.Entities.Characters.Enemy.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Tower
{
    public class TowerTargetFindingComponent : MonoBehaviour
    {
        [Title("Debugging", titleAlignment: TitleAlignments.Centered)]
        [SerializeField, ReadOnly]
        private bool _hasTarget;
        [SerializeField]
        private float _searchRange;
        [SerializeField]
        private float _searchInerval;
        [SerializeField, ReadOnly]
        private float _nextClosestCheckTime;

        private WorldEnemy _worldEnemy;
        private GameObject _enemyTarget;

        public bool HasTarget => _hasTarget;
        public GameObject TargetEnemy => _enemyTarget;

        public void UpdateComponent()
        {
            if (Time.time >= _nextClosestCheckTime + _searchInerval)
            {
                //FindClosestTargetInternal();
                _nextClosestCheckTime = Time.time;
            }
        }

        private void FindClosestTargetInternal()
        {
            if(_hasTarget == false)
            {
                var enemies = _worldEnemy.GetEnemies();
                ValidateClosestTarget(enemies.AsReadOnlySpan());
            }
            else
            {
                float currentDistance = Vector3.Distance(transform.position, _enemyTarget.transform.position);
                if (currentDistance > _searchRange || _enemyTarget.activeSelf == false)
                {
                    _enemyTarget = default(GameObject);
                    _hasTarget = false;
                }
            }
        }

        private void ValidateClosestTarget(ReadOnlySpan<EnemyAIController> enemies)
        {
            bool flag = false;
            int lenght = enemies.Length;
            int closestIndex = 0;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < lenght; i++)
            {
                var enemy = enemies[i];

                float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);

                if (currentDistance < closestDistance && currentDistance < _searchRange)
                {
                    flag = true;
                    closestIndex = i;
                    closestDistance = currentDistance;
                }
            }

            _enemyTarget = flag ? enemies[closestIndex].gameObject : default;

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _searchRange);
        }
    }
}
