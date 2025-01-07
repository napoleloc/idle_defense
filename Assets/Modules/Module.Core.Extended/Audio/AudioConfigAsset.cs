using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Core.Extended.Audio
{
    [CreateAssetMenu]
    [InlineEditor(InlineEditorModes.GUIAndPreview)]
    public class AudioConfigAsset : ScriptableObject
    {
        [TableList]
        [SerializeField] 
        private AudioContainerConfig[] _containers;

        public ReadOnlyMemory<AudioContainerConfig> Containers
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _containers;
        }
    }

    [System.Serializable]
    public class AudioContainerConfig
    {
        [SerializeField] 
        private AudioContainerType _containerType;
        [SerializeField] 
        private string _containerName;

        public AudioContainerType ContainerType => _containerType;
        public string ContainerName => _containerName;
    }
}
