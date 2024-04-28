using System;
using System.Linq;
using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ginko.StateMachineSystem
{
    public class P_IdleState : IdleState
    {
        private Player player;

        public P_IdleState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            player = (Player)entity;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            IsToAttackState = player.IsAttackInput;
            IsToMoveState = player.MoveDirection != Vector2.zero;
        }
    }
}
