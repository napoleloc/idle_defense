using UnityEngine;

namespace Module.Core.KCC
{
    public interface IMoverController
    {
        /// <summary>
        /// This is called to let you tell the PhysicsMover where it should be right now
        /// </summary>
        void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime);
    }
}
