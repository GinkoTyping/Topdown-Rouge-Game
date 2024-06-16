using Ginko.PlayerSystem;
using Ginko.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public class P_AttackState : AttackState
    {
        private Weapon weapon;
        private Player player;

        public P_AttackState(Entity entity, FiniteStateMachine stateMachine, Weapon weapon) : base(entity, stateMachine)
        {
            this.weapon = weapon;
            weapon.OnExit += ExitHandler;

            player = (Player)entity;
        }
        public override void Enter()
        {
            base.Enter();

            weapon.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            IsToIdleState = player.MoveDirection == Vector2.zero;
            IsToMoveState = player.MoveDirection != Vector2.zero;
        }

        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.MeleeAttack;
        }

        private void ExitHandler()
        {
            IsAnimationFinished = true;
        }
    }
}
