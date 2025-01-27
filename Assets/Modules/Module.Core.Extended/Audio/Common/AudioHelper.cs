using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Module.Core.Extended.Audio.PubSub;
using Module.Core.Extended.PubSub;

namespace Module.Core.Extended.Audio
{
    public static class AudioHelper
    {
        #region    SOUND_HELPER
        #endregion ============

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlaySound(AudioType audioType, float fadeInTime = 0)
            => WorldMessenger.Publisher.AudioScope()
            .Publish(new PlaySoundMessage(audioType, fadeInTime));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask PlaySoundAsync(AudioType audioType, float fadeInTime, CancellationToken token = default)
            => await WorldMessenger.Publisher.AudioScope()
            .PublishAsync(new AsyncMessage<PlaySoundMessage>(new PlaySoundMessage(audioType, fadeInTime)), token);

        #region    MUSIC_HELPER
        #endregion ============

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlayMusic(AudioType audioType, float fadeInTime = 0)
            => WorldMessenger.Publisher.AudioScope()
            .Publish(new PlayMusicMessage(audioType, fadeInTime));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask PlayMusicAsync(AudioType audioType, float fadeInTime, CancellationToken token = default)
            => await WorldMessenger.Publisher.AudioScope()
            .PublishAsync(new AsyncMessage<PlayMusicMessage>(new PlayMusicMessage(audioType, fadeInTime)), token);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PauseMusic(AudioType audioType, float fadeOutTime = 0)
            => WorldMessenger.Publisher.AudioScope()
            .Publish(new PauseMusicMessage(audioType,fadeOutTime));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnpauseMusic(AudioType audioType, float fadeInTime = 0)
            => WorldMessenger.Publisher.AudioScope()
            .Publish(new UnpauseMusicMessage(audioType,fadeInTime));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StopMusic(AudioType audioType, float fadeOutTime = 0)
            => WorldMessenger.Publisher.AudioScope()
            .Publish(new StopMusicMessage(audioType,fadeOutTime));
    }
}
