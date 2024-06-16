using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public abstract class IdleState : State
    {
        public bool IsToMoveState;
        public bool IsToAttackState;
        public bool IsToHostileDetectedState;
        public IdleState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.Idle;
        }

        public override void Enter()
        {
            base.Enter();

            IsToHostileDetectedState = false;
            IsToAttackState = false;
            IsToMoveState = false;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsToHostileDetectedState)
            {
                StateMachine.ChangeState(Entity.HostileDetectedState);
            }
            else if(IsToAttackState)
            {
                StateMachine.ChangeState(Entity.MeleeAttackState);
            }
            else if (IsToMoveState)
            {
                StateMachine.ChangeState(Entity.MoveState);
            }
        }
    }
}
