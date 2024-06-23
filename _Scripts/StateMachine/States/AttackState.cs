using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginko.StateMachineSystem
{
    public abstract class AttackState : State
    {
        public bool IsToIdleState;
        public bool IsToMoveState;
        public bool IsToHostileDetectedState;
        public bool IsToRangedAttackState;
        public bool IsToMeleeAttackState;
        public AttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            IsToIdleState = false;
            IsToMoveState= false;

            IsToHostileDetectedState = false;

            IsToRangedAttackState = false;
            IsToMeleeAttackState = false;

            Entity.Movement.SetVelocityZero();
            Entity.Anim.SetInteger("AttackCounter", 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsAnimationFinished)
            {
                if (IsToIdleState)
                {
                    StateMachine.ChangeState(Entity.IdleState);
                }
                else if (IsToMoveState)
                {
                    StateMachine.ChangeState(Entity.MoveState);
                }
                else if (IsToRangedAttackState)
                {
                    StateMachine.ChangeState(Entity.RangedAttackState);
                }
                else if (IsToMeleeAttackState)
                {
                    StateMachine.ChangeState(Entity.MeleeAttackState);
                }
                else if (IsToHostileDetectedState)
                {
                    StateMachine.ChangeState(Entity.HostileDetectedState);
                }
            }
        }
    }
}
