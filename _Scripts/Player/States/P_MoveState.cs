using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public class P_MoveState : MoveState
    {
        private Player player;

        public P_MoveState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            player = (Player)entity;
        }

        protected override bool IsToIdleState()
        {
            return player.MoveDirection == Vector2.zero;
        }

        protected override void SetVelocity()
        {
            Entity.Movement.SetVelocity(Entity.EntityData.moveVelocity, player.MoveDirection);
        }
        protected override bool IsToAttackState()
        {
            return player.IsAttackInput;
        }
    }
}
