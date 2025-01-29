using Module.Core.HFSM.States;

namespace Module.Entities.Characters.Enemy.AI.Minion
{
    public class MinionAIController : EnemyAIController
    {
        protected override void OnEnterNormalAttackState(State<EnemyState, EnemyStateEvent> state)
        {
        }

        protected override void OnExitNormalAttackState(State<EnemyState, EnemyStateEvent> state)
        {
           
        }
    }
}
