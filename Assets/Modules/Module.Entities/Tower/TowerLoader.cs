using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.AddressableKeys;
using EncosyTower.Modules.Vaults;
using Module.Entities.Tower.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Module.Entities.Tower
{
    public class TowerLoader : MonoBehaviour
    {
        public static readonly Id<TowerLoader> PresetId = default;

        [SerializeField]
        private TowerDatabase _database;    

        private GameObject _usedObject;
        private bool _loaded;

        public TowerDatabase Database
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _database;
        }

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

        public void Load()
        {

        }

        public void Unload()
        {
            if (_usedObject.IsValid())
            {
                Addressables.ReleaseInstance(_usedObject);
            }
        }

        private async UniTask LoadAsyncInternal(ushort id)
        {
            if (_loaded)
            {
                Unload();
            }

            var towerHandle = new AddressableKey<GameObject>(TowerNames.Format(id));

            _usedObject = await towerHandle.InstantiateAsync(gameObject.scene, trimCloneSuffix: true);
        }
    }
}
