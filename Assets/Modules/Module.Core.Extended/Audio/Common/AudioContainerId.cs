using Sirenix.OdinInspector;

namespace Module.Core.Extended.Audio
{
    [System.Serializable]
    public struct AudioContainerId
    {
        [HorizontalGroup]
        [HideLabel]
        public string name;

        [HorizontalGroup]
        [HideLabel]
        public int id;

        public AudioContainerId(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
    }
}
