using System;
using EncosyTower.Modules.EnumExtensions;
using UnityEngine;

namespace Module.Core.Extended.Audio
{
    [EnumExtensions]
    public enum AudioType
    {
        button,
        claim
    }

    [Serializable]
    public struct AudioEntry
    {
        [SerializeField]
        private AudioType _type;
        [SerializeField]
        private string _name;

        public AudioType Type => _type;
        public string Name => _name;

#if UNITY_EDITOR
#endif
    }
}
