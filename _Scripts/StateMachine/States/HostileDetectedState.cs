using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public abstract class HostileDetectedState : State
    {
        public bool IsToMeleeAttackState;
        public bool IsToRangedAttackState;
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

            IsToMeleeAttackState = false;
            IsToRangedAttackState = false;
            IsToIdleState = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsToMeleeAttackState)
            {
                StateMachine.ChangeState(Entity.MeleeAttackState);
            }
            else if (IsToRangedAttackState)
            {
                StateMachine.ChangeState(Entity.RangedAttackState);
            }
            else if (IsToIdleState)
            {
                StateMachine.ChangeState(Entity.IdleState);
            }
        }
    }
}