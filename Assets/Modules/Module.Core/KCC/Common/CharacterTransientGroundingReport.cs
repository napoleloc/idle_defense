using UnityEngine;

namespace Module.Core.KCC
{
    /// <summary>
    /// Contains the simulation-relevant information for the motor's grounding status
    /// </summary>
    public struct CharacterTransientGroundingReport
    {
        public bool foundAnyGround;
        public bool isStableOnGround;
        public bool snappingPrevented;
        public Vector3 groundNormal;
        public Vector3 innerGroundNormal;
        public Vector3 outerGroundNormal;

        public void CopyFrom(CharacterGroundingReport groundingReport)
        {
            foundAnyGround = groundingReport.foundAnyGround;
            isStableOnGround = groundingReport.isStableOnGround;
            snappingPrevented = groundingReport.snappingPrevented;
            groundNormal = groundingReport.groundNormal;
            innerGroundNormal = groundingReport.innerGroundNormal;
            outerGroundNormal = groundingReport.outerGroundNormal;
        }
    }
}
