using System.Threading;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.PubSub;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.Audio.Music;
using Module.Core.Extended.Audio.PubSub;
using Module.Core.Extended.Audio.Sound;
using Module.Core.Extended.PubSub;
using UnityEngine;

namespace Module.Core.Extended.Audio
{
    public class WorldAudio : MonoBehaviour
    {
        public static readonly Id<WorldAudio> PresetId = default;

        private MusicChannelContainer _musicChannelContainer;
        private SoundChannelContainer _soundChannelContainer;

        private void Awake()
        {
            _musicChannelContainer = GetComponentInChildren<MusicChannelContainer>();
            _soundChannelContainer = GetComponentInChildren<SoundChannelContainer>();
        }

        private void Start()
        {
            InitializeAndForget().Forget();
        }

        private void OnDestroy()
        {
            _musicChannelContainer.Deinitialize();
            _soundChannelContainer.Deinitialize();

            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private async UniTaskVoid InitializeAndForget()
        {
            await _musicChannelContainer.InitializeAsync();
            await _soundChannelContainer.InitializeAsync();

            var subscriber = WorldMessenger.Subscriber.AudioScope().WithState(this);

            subscriber.Subscribe<AsyncMessage<PlaySoundMessage>>(static (state, msg, token) => state.HandleAsync(msg, token));
            subscriber.Subscribe<PlaySoundMessage>(static (state, msg) => state.Handle(msg));

            subscriber.Subscribe<AsyncMessage<PlayMusicMessage>>(static (state, msg, token) => state.HandleAsync(msg, token));
            subscriber.Subscribe<PlayMusicMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<PauseMusicMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<UnpauseMusicMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<StopMusicMessage>(static (state, msg) => state.Handle(msg));

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private async UniTask HandleAsync(AsyncMessage<PlaySoundMessage> asyncMessage, CancellationToken token)
        {
            var message = asyncMessage.Message;

            await _soundChannelContainer.PlaySoundAsync(message.AudioType, message.FadeInTime, token);
        }

        private void Handle(PlaySoundMessage message)
            => _soundChannelContainer.PlaySound(message.AudioType, message.FadeInTime);

        private async UniTask HandleAsync(AsyncMessage<PlayMusicMessage> asyncMessage, CancellationToken token)
        {
            var message = asyncMessage.Message;

            await _musicChannelContainer.PlayMusicAsync(message.AudioType, message.FadeInTime, token);
        }

        private void Handle(PlayMusicMessage message)
            => _musicChannelContainer.PlayMusic(message.AudioType, message.FadeInTime);

        private void Handle(PauseMusicMessage messgae)
            => _musicChannelContainer.PauseMusic(messgae.AudioType, messgae.FadeOutTime);

        private void Handle(UnpauseMusicMessage message)
            => _musicChannelContainer.UnpauseMusic(message.AudioType, message.FadeInTime);

        private void Handle(StopMusicMessage message)
            => _musicChannelContainer.StopMusic(message.AudioType, message.FadeOutTime);
    }
}
