using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public abstract class HostileDetectedState : State
    {
        public bool IsToAttackState;
        public bool IsToIdleState;
        public HostileDetectedState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.HostileDetected;
        }

        public override void Enter()
        {
            base.Enter();

            IsToAttackState = false;
            IsToIdleState = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsToAttackState)
            {
                StateMachine.ChangeState(Entity.AttackState);
            }
            else if (IsToIdleState)
            {
                StateMachine.ChangeState(Entity.IdleState);
            }
        }
    }
}