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

            CheckIfFlip();
            interaction.CheckIfShowInteractHint();

            if (player.MoveDirection != Vector2.zero)
            {
                StateMachine.ChangeState(Entity.MoveState);
            }
            else if (player.InputHandler.Interact)
            {
                player.InputHandler.UseInteractSignal();
                interaction.InteractItem();
            } 
            else if (player.InputHandler.Switch)
            {
                player.InputHandler.UseSwitchSignal();
                interaction.SwitchInteratItem();
            }
        }

        private void CheckIfFlip()
        {
            if (player.IsAttackInput)
            {
                if (player.InputHandler.MousePosition.x > player.transform.position.x && player.Movement.FacingDirection < 0)
                {
                    player.Movement.Flip();
                }
            }
        }
    }
}
