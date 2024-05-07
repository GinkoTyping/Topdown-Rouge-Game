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
        public Interaction interaction;
        public bool isInteracting;

        public P_IdleState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            player = (Player)entity;
            interaction = entity.Core.GetCoreComponent<Interaction>();
        }

        public override void Enter()
        {
            base.Enter();

            isInteracting = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            IsToAttackState = player.IsAttackInput;
            IsToMoveState = player.MoveDirection != Vector2.zero;

            interaction.CheckIfShowInteractHint();

            if (player.InputHandler.Interact)
            {
                player.InputHandler.UseInteractSignal();
                interaction.InteractItem();
            }
        }
    }
}
