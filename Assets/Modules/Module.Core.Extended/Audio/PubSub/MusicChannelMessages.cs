using EncosyTower.Modules.PubSub;

namespace Module.Core.Extended.Audio.PubSub
{
    public readonly struct PlayMusicMessage : IMessage
    {
        public readonly AudioType AudioType;
        public readonly float FadeInTime;

        public PlayMusicMessage(AudioType audioType, float fadeInTime = 0)
        {
            AudioType = audioType;
            FadeInTime = fadeInTime;
        }
    }

    public readonly struct PauseMusicMessage : IMessage
    {
        public readonly AudioType AudioType;
        public readonly float FadeOutTime;

        public PauseMusicMessage(AudioType audioType, float fadeOutTime = 0)
        {
            AudioType = audioType;
            FadeOutTime = fadeOutTime;
        }
    }

    public readonly struct UnpauseMusicMessage : IMessage
    {
        public readonly AudioType AudioType;
        public readonly float FadeInTime;

        public UnpauseMusicMessage(AudioType audioType, float fadeInTime = 0)
        {
            AudioType = audioType;
            FadeInTime = fadeInTime;
        }
    }

    public readonly struct StopMusicMessage : IMessage
    {
        public readonly AudioType AudioType;
        public readonly float FadeOutTime;

        public StopMusicMessage(AudioType audioType, float fadeOutTime = 0)
        {
            AudioType = audioType;
            FadeOutTime = fadeOutTime;
        }
    }
}
