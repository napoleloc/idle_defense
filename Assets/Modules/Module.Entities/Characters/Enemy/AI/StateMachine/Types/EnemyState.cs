namespace Module.Entities.Characters.Enemy.AI
{
    public enum EnemyState : byte
    {
        Appear,
        Idle,
        Chase,

        NormalAttack,
        SpecialAttack,

        Dying,
        Dead,
    }
}
