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
        public AttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            IsToIdleState = false;
            IsToMoveState= false;
            IsToHostileDetectedState = false;
        }

        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.Attack;
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
                else if (IsToHostileDetectedState)
                {
                    StateMachine.ChangeState(Entity.HostileDetectedState);
                }
            }
        }
    }
}
