using EncosyTower.Modules;
using EncosyTower.Modules.Vaults;
using Module.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Module.GameCommon
{
    public class InputReceiver : MonoBehaviour
    {
        public static readonly Id<InputReceiver> PresetId = default;

        private static Vector2 s_inputValue;

        private InputSystem_Actions _inputSystem;

        public static Vector2 InputValue => s_inputValue;

        private void Awake()
        {
            _inputSystem = new();

            Initialize();
        }

        private void OnDestroy()
        {
            _inputSystem.Dispose();

            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private void Initialize()
        {
            _inputSystem.Player.Move.performed += Handle;
            _inputSystem.Enable();

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void Handle(InputAction.CallbackContext context)
            => s_inputValue = context.ReadValue<Vector2>();
    }
}
