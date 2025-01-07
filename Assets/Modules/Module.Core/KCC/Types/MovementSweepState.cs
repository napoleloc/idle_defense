namespace Module.Core.KCC
{
    public enum MovementSweepState : byte
    {
        Initial,
        AfterFirstHit,
        FoundBlockingCrease,
        FoundBlockingCorner,
    }
}
