using System.Threading;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Core;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

namespace Module.Entities.Characters.Enemy
{
    public class EnemyProgressManager : MonoBehaviour
    {
        public static readonly Id<EnemyProgressManager> PresetId = default;

        private EnemyPooler _enemyPooler;
        private CancellationTokenSource _cts;

        private NativeList<Vector3> _positions;

        private Vector3 _point = Vector3.zero;
        private uint _capacity = 10;
        private uint _loopCount;

        private async void Start()
        {
            await InitializeAsync(default);
            _positions = new(Allocator.Persistent);
        }

        private void OnDestroy()
        {
            if (_positions.IsCreated)
            {
                _positions.Dispose();
            }
            _positions = default;

            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private async UniTask InitializeAsync(CancellationToken token)
        {
            await GlobalValueVault<bool>.WaitUntil(EnemyPooler.PresetId, true, token);
            
            if(GlobalObjectVault.TryGet(EnemyPooler.PresetId, out _enemyPooler))
            {
                GlobalObjectVault.TryAdd(PresetId, this);
                GlobalValueVault<bool>.TrySet(PresetId, true);
            }
        }

        [Button(buttonSize: 35)]
        public void BeginProgress()
        {
            _loopCount = 0;
            _point.GetRandomPointsOnEdge(10, ref _positions, _capacity);
            ProgressAndForget().Forget();
        }

        [Button(buttonSize: 35)]
        public void CancelProgress()
        {
            _cts.Cancel();
            _loopCount = 0;
        }

        private async UniTask ProgressAndForget()
            => await ProgressAsyncInternal();

        private async UniTask ProgressAsyncInternal()
        {
            if(_loopCount >= 10)
            {
                return;
            }

            var position = _positions[(int)Mathf.Clamp(_loopCount, 0, _positions.Length)];
            var rotation = (_point - position).normalized;

            _enemyPooler.GetFromPoolBy(GameCommon.EnemyType.Minion, position, rotation);
            _loopCount++;

            RenewCts();
            await UniTask.WaitForSeconds(1, cancellationToken: _cts.Token);
            ProgressAndForget().Forget();
        }

        private void RenewCts()
        {
            _cts ??= new();
            if (_cts.IsCancellationRequested)
            {
                _cts.Dispose();
                _cts = new();
            }
        }
    }
}
