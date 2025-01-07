using UnityEngine;

namespace Module.Core.Extended.Camera
{
    internal class CameraShiftComponent : MonoBehaviour
    {
        private float _forwardX;
        private float _forwardZ;
        private float _forwardLerpMultiplier;

        private float _targetShiftX;
        private float _targetShiftZ;
        private float _targetShiftLerpMultiplier;

        private Vector3 _forward = Vector3.zero;
        private Vector3 _targetDirection;

        private Transform _target;

        public void SetTarget(Transform target)
            => _target = target;

        public void UpdateCameraShift(Transform mainTarget, out Vector3 newPosition)
        {
            var z = mainTarget.forward.z * _forwardZ;
            var x = mainTarget.forward.x * _forwardX;

            _forward = Vector3.Lerp(_forward, new Vector3(x, 0, z), Time.deltaTime * _forwardLerpMultiplier);

            var currentEnemyDirection = _target ? (_target.transform.position - mainTarget.position).normalized : Vector3.zero;

            currentEnemyDirection.x *= _targetShiftX;
            currentEnemyDirection.z *= _targetShiftZ;

            _targetDirection = Vector3.Lerp(_targetDirection, currentEnemyDirection, Time.deltaTime * _targetShiftLerpMultiplier);

            newPosition = mainTarget.position + _forward + _targetDirection;
        }
    }
}
