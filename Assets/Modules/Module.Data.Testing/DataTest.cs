using Module.Data.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Module.Data.Testing
{
    public class DataTest : MonoBehaviour
    {
        [Button]
        private void Run()
        {
            var runtimeDatabase =  WorldRuntimeData.RuntimeDatabase;
        }
    }
}
