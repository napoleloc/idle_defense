using System;
using UnityEngine;

namespace Module.Core.Extended.Audio
{
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
