using Ginko.CoreSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public abstract class MoveState : State
    {
        public MoveState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.Move;
        }

        public override void Enter()
        {
            base.Enter();

            SetVelocity();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            SetVelocity();

            if (IsToAttackState())
            {
                StateMachine.ChangeState(Entity.MeleeAttackState);
            }
            if (IsToIdleState())
            {
                StateMachine.ChangeState(Entity.IdleState);
            }
        }
        protected abstract void SetVelocity();
        protected abstract bool IsToIdleState();
        protected abstract bool IsToAttackState();

    }
}
