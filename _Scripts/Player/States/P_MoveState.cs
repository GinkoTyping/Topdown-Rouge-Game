using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public class P_MoveState : MoveState
    {
        private Player player;
        private float dashEndTime;

        public P_MoveState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            player = (Player)entity;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.InputHandler.Dash)
            {
                dashEndTime = Time.time + Entity.EntityData.dashDuaration;
            }
        }

        protected override bool IsToIdleState()
        {
            return player.MoveDirection == Vector2.zero;
        }

        protected override void SetVelocity()
        {
            float velocity = Time.time <= dashEndTime ? Entity.EntityData.dashVelocity : Entity.EntityData.moveVelocity;
            Entity.Movement.SetVelocity(velocity, player.MoveDirection);
        }
        protected override bool IsToAttackState()
        {
            return player.IsAttackInput;
        }
    }
}
