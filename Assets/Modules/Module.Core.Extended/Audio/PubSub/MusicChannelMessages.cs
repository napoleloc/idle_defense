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
}
