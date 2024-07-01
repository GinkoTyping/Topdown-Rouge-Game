using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public abstract class IdleState : State
    {
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

            Entity.Movement.SetVelocityZero();
        }
    }
}
