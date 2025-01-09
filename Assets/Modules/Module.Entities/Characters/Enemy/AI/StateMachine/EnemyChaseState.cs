using JetBrains.Annotations;
using UnityEngine;

namespace Module.Entities.Characters.Enemy.AI
{
    public class EnemyChaseState : EnemyBaseState
    {
        private readonly CharacterController _characterController;
        private readonly CharacterPhysicsComponent _characterPhysicsComponent;

        private Vector3 _destination = Vector3.zero;
        private Vector3 _direction;

        public EnemyChaseState([NotNull] EnemyAIController controller, bool needsExitTime) : base(controller, needsExitTime)
        {
            _characterController = controller.CharacterController;
            _characterPhysicsComponent = controller.CharacterPhysicsComponent;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            NavMeshAgent.isStopped = false;
            NavMeshAgent.updatePosition = false;

            NavMeshAgent.SetDestination(_destination);

            CharacterAnimationComponent.CrossFadeAnim(GameCommon.Animation.CharAnim.Walk);
        }

        public override void OnLogic()
        {
            var position = Controller.transform.position;
            var distance = Vector3.Distance(position, _destination);
            if(NavMeshAgent.stoppingDistance >= distance)
            {
                StateMachine.StateCanExit();
                return;
            }

            _direction = (NavMeshAgent.nextPosition - position).normalized;

            _characterPhysicsComponent.ApplyGravity();
            _characterController.Move(NavMeshAgent.speed * Time.deltaTime * _direction);
        }
    }
}
