using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Cysharp.Threading.Tasks;
using EncosyTower.Modules;
using EncosyTower.Modules.Logging;
using EncosyTower.Modules.PubSub;
using EncosyTower.Modules.Vaults;
using Module.Core.Extended.Audio.Musics;
using Module.Core.Extended.Audio.PubSub;
using Module.Core.Extended.Audio.Sounds;
using Module.Core.Extended.PubSub;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Audio
{
    public class WorldAudio : MonoBehaviour
    {
        public static readonly Id<WorldAudio> PresetId = default;

        [SerializeField]
        private AudioConfigAsset _audioConfigAsset;

        [Title("Audio Container IDs", titleAlignment: TitleAlignments.Centered)]
        [InlineProperty]
        [LabelText("Sound")]
        [SerializeField] 
        private AudioContainerId _soundId;
        [InlineProperty]
        [LabelText("Music")]
        [SerializeField] 
        private AudioContainerId _musicId;

        public static float Volume;
        public static float SoundVolume;
        public static float MusicVolume;

        private SoundContainer _soundContainer;
        private MusicContainer _musicContainer;

        public static void SetAllVolumes(float value)
        {
            Volume = value;
            SoundVolume = value;
            MusicVolume = value;
        }

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            SetAllVolumes(1);
        }
#endif

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        private void Initialize()
        {
            var scene = gameObject.scene;
            var configs = _audioConfigAsset.Containers.Span;

            for (int i = 0; i < configs.Length; i++)
            {
                var config = configs[i];
                switch (config.ContainerType)
                {
                    case AudioContainerType.Sound:
                    {
                        _soundContainer = SoundContainer.CreateInstance(config, transform);
                        break;
                    }

                    case AudioContainerType.Music:
                    {
                        _musicContainer = MusicContainer.CreateInstance(config, transform);
                        break;
                    }
                }
            }

            OnInitialize();
        }

        private void OnInitialize()
        {
            var subscriber = WorldMessenger.Subscriber.AudioScope().WithState(this);

            subscriber.Subscribe<AsyncMessage<PlaySoundMessage>>(static (state, msg) => state.HandleAsync(msg));
            subscriber.Subscribe<PlaySoundMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<PauseSoundMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<UnpauseSoundMessage>(static (state, msg) => state.Handle(msg));
            subscriber.Subscribe<StopSoundMessage>(static (state, msg) => state.Handle(msg));

            subscriber.Subscribe<AsyncMessage<PlayMusicMessage>>(static (state, msg) => state.HandleAsync(msg));
            subscriber.Subscribe<PlayMusicMessage>(static (state, msg) => state.Handle(msg));

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        #region    SOUND_EVENT_METHODS
        #endregion ===================

        private async UniTask HandleAsync(AsyncMessage<PlaySoundMessage> asyncMessage)
        {
            if (_soundContainer.IsInvalid())
            {
                LogErrorContainerIsInvalid(_soundContainer);
                return;
            }

            var message = asyncMessage.Message;

            await _soundContainer.PlaySoundAsync();
        }

        private void Handle(PlaySoundMessage message)
        {
            if (_soundContainer.IsInvalid())
            {
                LogErrorContainerIsInvalid(_soundContainer);
                return;
            }

            _soundContainer.PlaySound();
        }

        private void Handle(PauseSoundMessage messgae)
        {
            if (_soundContainer.IsInvalid())
            {
                LogErrorContainerIsInvalid(_soundContainer);
                return;
            }

            _soundContainer.PauseSound();
        }

        private void Handle(UnpauseSoundMessage message)
        {
            if (_soundContainer.IsInvalid())
            {
                LogErrorContainerIsInvalid(_soundContainer);
                return;
            }

            _soundContainer.UnpauseSound();
        }

        private void Handle(StopSoundMessage message)
        {
            if (_soundContainer.IsInvalid())
            {
                LogErrorContainerIsInvalid(_soundContainer);
                return;
            }

            _soundContainer.StopSound();
        }

        #region    MUSIC_EVENT_METHODS
        #endregion ===================

        private async UniTask HandleAsync(AsyncMessage<PlayMusicMessage> asyncMessage)
        {
            if (_musicContainer.IsInvalid())
            {
                LogErrorContainerIsInvalid(_musicContainer);
                return;
            }
           

            var message = asyncMessage.Message;

            await _musicContainer.PlayMusicAsync();
        }

        private void Handle(PlayMusicMessage message)
        {
            if (_musicContainer.IsInvalid())
            {
                LogErrorContainerIsInvalid(_musicContainer);
                return;
            }

            _musicContainer.PlayMusic();
        }

        #region    DEBUGGING_METHODS
        #endregion =================

        [HideInCallstack, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogErrorContainerIsInvalid([NotNull] AudioContainer container)
            => DevLoggerAPI.LogError(container, "Container is invalid!");
    }
}
