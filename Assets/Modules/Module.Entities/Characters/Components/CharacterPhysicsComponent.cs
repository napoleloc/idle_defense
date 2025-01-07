using EncosyTower.Modules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Entities.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterPhysicsComponent : MonoBehaviour, IEntityComponent, IInitialization
    {
        [FoldoutGroup("Settings")]
        [SerializeField] private float _groundGravity;
        [FoldoutGroup("Settings")]
        [SerializeField] private float _fallGravity = -10f;
        [FoldoutGroup("Settings")]
        [SerializeField] private float _terminalVelocity = -50f;

        [FoldoutGroup("Debugging")]
        [SerializeField, ReadOnly] private bool _isActive;
        [FoldoutGroup("Debugging")]
        [SerializeField, ReadOnly] private bool _isGrounded;
        [FoldoutGroup("Debugging")]
        [SerializeField, ReadOnly] private float _verticalVelocity;

        // Components
        private CharacterController _characterController;

        public void InitializeDependencies()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Initialize()
        {
            _isActive = _characterController.IsValid();
        }

        public void Activate()
        {
            _isActive = true;
            _characterController.enabled = _isActive;
        }

        public void Deactivate()
        {
            _isActive = false;
            _characterController.enabled = _isActive;
        }

        public void ApplyGravity()
        {
            if (_isActive)
            {
                ApplyGravityInternal();
            }
        }

        private void ApplyGravityInternal()
        {
            _isGrounded = _characterController.isGrounded;
            if (_isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity = _groundGravity;
            }
            else
            {
                _verticalVelocity += _fallGravity * 2 * Time.deltaTime;
                _verticalVelocity = Mathf.Max(_verticalVelocity, _terminalVelocity);
            }

            _characterController.Move(new Vector3(0.0F, _verticalVelocity, 0.0F) * Time.deltaTime);
        }


    }
}
