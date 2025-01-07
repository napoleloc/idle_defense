using Cysharp.Threading.Tasks;
using Module.Core.Extended.Audio.PubSub;
using Module.Core.Extended.PubSub;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Audio
{
    public class AudioTest : MonoBehaviour
    {
        [Button(buttonSize: 35)]
        public void PlaySound()
        {
            WorldMessenger.Publisher.AudioScope().Publish(new PlaySoundMessage());
        }

        public async UniTask PlaySoundAsync()
        {
            await WorldMessenger.Publisher.AudioScope().PublishAsync(new AsyncMessage<PlaySoundMessage>(new PlaySoundMessage()));
        }

        public void PauseSound()
        {

        }

        public void UnpauseSound()
        {

        }

        public void StopSound()
        {

        }

        [Button(buttonSize: 35)]
        public void PlayMusic()
        {
            WorldMessenger.Publisher.AudioScope().Publish(new PlayMusicMessage());
        }

        public async UniTask PlayMusicAsync()
        {
            await WorldMessenger.Publisher.AudioScope().PublishAsync(new AsyncMessage<PlayMusicMessage>(new PlayMusicMessage()));
        }
    }
}
