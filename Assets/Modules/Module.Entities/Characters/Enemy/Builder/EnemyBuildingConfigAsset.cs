using System;
using Module.GameCommon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Characters.Enemy.Builder
{
    [CreateAssetMenu]
    public class EnemyBuildingConfigAsset : ScriptableObject
    {
        [TableList]
        [SerializeField]
        private BuildingConfig[] _buildingConfigs;

        public ReadOnlyMemory<BuildingConfig> BuildingConfigs
        {
            get => _buildingConfigs;
        }
    }

    [System.Serializable]
    public struct BuildingConfig
    {
        [SerializeField]
        private EnemyType _type;
        [SerializeField]
        private string _name;

        public EnemyType Type => _type;
        public string Name => _name;
    }
}
