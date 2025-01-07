using Cysharp.Threading.Tasks;
using Module.Core.Extended.PubSub;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Module.Core.Extended.Camera
{
    public class CameraTest : MonoBehaviour
    {
        public void Shake()
        {
            var message = new ShakeCameraMessage(1,1,1,1);
            WorldMessenger.Publisher.CameraScope()
                .Publish(message);
        }

        public async UniTask ShakeAsync()
        {
            var message = new ShakeCameraMessage(1,1,1,1);
            await WorldMessenger.Publisher.CameraScope()
                .PublishAsync(new AsyncMessage<ShakeCameraMessage>(message));
        }

        public void EnableCameraShift()
        {
            WorldMessenger.Publisher.CameraScope()
               .Publish(new SwtichCameraShiftMessage(true));
        }

        public void DisableCameraShift()
        {
            WorldMessenger.Publisher.CameraScope()
               .Publish(new SwtichCameraShiftMessage(false));
        }
    }
}
