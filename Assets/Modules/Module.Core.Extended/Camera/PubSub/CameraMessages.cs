using EncosyTower.Modules.PubSub;
using Unity.Cinemachine;

namespace Module.Core.Extended.Camera
{
    public readonly struct RegisterCameraMessage : IMessage
    {
        public readonly ICinemachineCamera Cinemachine;

        public RegisterCameraMessage(ICinemachineCamera cinemachine)
        {
            Cinemachine = cinemachine;
        }

        public static RegisterCameraMessage From<T>(T cinemachine) where T : ICinemachineCamera
            => new(cinemachine);
    }

    public readonly struct UnregisterCameraMessage : IMessage
    {
        public readonly ICinemachineCamera Cinemachine;

        public UnregisterCameraMessage(ICinemachineCamera cinemachine)
        {
            Cinemachine = cinemachine;
        }

        public static UnregisterCameraMessage From<T>(T cinemachine) where T : ICinemachineCamera
            => new(cinemachine);
    }

    public readonly struct SwitchToCameraMessage : IMessage
    {
        public readonly int Index;

        public SwitchToCameraMessage(int index)
        {
            Index = index;
        }

    }

    public readonly struct ShakeCameraMessage : IMessage
    {
        public readonly float FadeInTime;
        public readonly float FadeOutTime;
        public readonly float Duration;
        public readonly float Gain;

        public ShakeCameraMessage(float fadeInTime, float fadeOutTime, float duration, float gain)
        {
            FadeInTime = fadeInTime;
            FadeOutTime = fadeOutTime;
            Duration = duration;
            Gain = gain;
        }
    }

    public readonly struct SwtichCameraShiftMessage : IMessage
    {
        public readonly bool State;
        public SwtichCameraShiftMessage(bool state)
        {
            State = state;
        }
    }
}
