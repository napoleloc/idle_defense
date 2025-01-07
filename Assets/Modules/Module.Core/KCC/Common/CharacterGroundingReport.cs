using UnityEngine;

namespace Module.Core.KCC
{
    /// <summary>
    /// Contains all the information for the motor's grounding status
    /// </summary>
    public struct CharacterGroundingReport
    {
        public bool foundAnyGround;
        public bool isStableOnGround;
        public bool snappingPrevented;
        public Vector3 groundNormal;
        public Vector3 innerGroundNormal;
        public Vector3 outerGroundNormal;

        public Collider groundCollider;
        public Vector3 groundPoint;

        public void CopyFrom(CharacterTransientGroundingReport transientGroundingReport)
        {
            foundAnyGround = transientGroundingReport.foundAnyGround;
            isStableOnGround = transientGroundingReport.isStableOnGround;
            snappingPrevented = transientGroundingReport.snappingPrevented;
            groundNormal = transientGroundingReport.groundNormal;
            innerGroundNormal = transientGroundingReport.innerGroundNormal;
            outerGroundNormal = transientGroundingReport.outerGroundNormal;

            groundCollider = null;
            groundPoint = Vector3.zero;
        }
    }

}
