using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.PubSub;
using EncosyTower.Modules.Vaults;
using LitMotion;
using Module.Core.Extended.PubSub;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace Module.Core.Extended.Camera
{
    public sealed class WorldCamera : MonoBehaviour
    {
        private const int DEFAULT_PRIORITY = 10;

        public static readonly Id<WorldCamera> PresetId = default;

        [SerializeField]
        private CameraConfigAsset _config;

        [BoxGroup("Debugging", centerLabel: true)]
        [SerializeField, ReadOnly] private bool _initialized;
        [BoxGroup("Debugging", centerLabel: true)]
        [SerializeField, ReadOnly] private bool _cameraShiftEnabled;
        [BoxGroup("Debugging", centerLabel: true)]
        [ShowInInspector, ReadOnly]
        private FasterList<CinemachineCamera> _cameras = new();

        private CameraShiftComponent _cameraShiftComponent;
        private CinemachineCamera _currentCinemachine;

        private Transform _mainTarget;
        private Transform _internalTarget;

        public ReadOnlyMemory<CinemachineCamera> Cameras
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _cameras.AsReadOnlyMemory();
        }

        private void Awake()
        {
            _cameras.AddRange(GetComponentsInChildren<CinemachineCamera>());
            _cameraShiftComponent = gameObject.GetOrAddComponent<CameraShiftComponent>();

            var subscriber =  WorldMessenger.Subscriber.CameraScope().WithState(this);
            subscriber.Subscribe<RegisterCameraMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<UnregisterCameraMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<SwitchToCameraMessage>(static (state, msg) => state.Handle(msg));

            subscriber.Subscribe<AsyncMessage<ShakeCameraMessage>>(static (state, msg) => state.HandleAsync(msg));
            subscriber.Subscribe<ShakeCameraMessage>(static (state, msg) => state.Handle(msg));

            subscriber.Subscribe<SwtichCameraShiftMessage>(static (state, msg) => state.Handle(msg));

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private void Update()
        {
            if(_initialized == false)
            {
                return;
            }

            if (_cameraShiftEnabled)
            {
                _cameraShiftComponent.UpdateCameraShift(_mainTarget, out var position);
                _internalTarget.position = position;
            }
            else
            {
                _internalTarget.position = _mainTarget.position;
            }
        }
        private void Initialize()
        {
            var cameras = Cameras.Span;
            var camerasLenght = cameras.Length;

            for (var i = 0; i < camerasLenght; i++)
            {
                var cam = cameras[i];
                cam.Priority = DEFAULT_PRIORITY;
            }

        }

        private void Handle(RegisterCameraMessage message)
        {
            if(message.Cinemachine is CinemachineCamera cinemachineCamera)
            {
                _cameras.Add(cinemachineCamera);
            }
        }

        private void Handle(UnregisterCameraMessage message)
        {
            if (message.Cinemachine is CinemachineCamera cinemachineCamera)
            {
                _cameras.Remove(cinemachineCamera);
            }
        }

        private void Handle(SwitchToCameraMessage message)
        {
            if (_currentCinemachine.IsValid())
            {
                _currentCinemachine.Priority = DEFAULT_PRIORITY;
            }

            _currentCinemachine = Cameras.Span[message.Index];
            _currentCinemachine.Priority = 100;
        }

        private async UniTask HandleAsync(AsyncMessage<ShakeCameraMessage> asyncMessage)
            => await ShakeCameraAsyncInternal(asyncMessage.Message);

        private void Handle(ShakeCameraMessage message)
            => ShakeCameraAndForget(message).Forget();

        private void Handle(SwtichCameraShiftMessage message)
            => _cameraShiftEnabled = message.State;

        private async UniTaskVoid ShakeCameraAndForget(ShakeCameraMessage message)
            => await ShakeCameraAsyncInternal(message);

        private async UniTask ShakeCameraAsyncInternal(ShakeCameraMessage message, CancellationToken token = default)
        {
            if (_currentCinemachine.IsValid())
            {
                if (_currentCinemachine.TryGetComponent<CinemachineBasicMultiChannelPerlin>(out var component))
                {
                    await LMotion.Create(0.0F, message.Gain, message.FadeInTime)
                        .Bind((fadeInValue) => component.AmplitudeGain = fadeInValue);

                    await LMotion.Create(message.Gain, 0.0F, message.FadeOutTime)
                        .WithDelay(message.Duration)
                        .Bind((fadeOutValue) => component.AmplitudeGain = fadeOutValue);
                }
            }
        }
    }
}
