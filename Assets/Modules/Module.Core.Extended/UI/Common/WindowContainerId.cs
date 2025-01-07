using System;
using Sirenix.OdinInspector;

namespace Module.Core.Extended.UI
{
    [Serializable]
    public struct WindowContainerId
    {
        [HorizontalGroup()]
        [HideLabel]
        public string name;
        [HorizontalGroup()]
        [HideLabel]
        public int id;

        public WindowContainerId(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
    }
}
