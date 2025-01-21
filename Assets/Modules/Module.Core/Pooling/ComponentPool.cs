using UnityEngine;

namespace Module.Core.Pooling
{
    public class ComponentPool<TPrefab, TComponent>
        where TPrefab : ComponentPrefab<TComponent>
        where TComponent : Component
    {
    }
}
