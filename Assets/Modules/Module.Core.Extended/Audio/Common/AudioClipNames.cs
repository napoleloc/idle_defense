namespace Module.Core.Extended.Audio
{
    public static class AudioClipNames
    {
        private const string FORMAT = "audio-clip-{0}";

        public static string AudioNameBy(AudioType audioType)
            => string.Format(FORMAT, audioType.ToStringFast());
    }
}
