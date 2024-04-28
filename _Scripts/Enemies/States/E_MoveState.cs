using System;
using System.Threading;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public class E_MoveState : MoveState
    {
        private Timer timer;
        public E_MoveState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

        }

        protected override bool IsToAttackState()
        {
            return false;
        }

        protected override bool IsToIdleState()
        {
            return false;
        }

        protected override void SetVelocity()
        {

        }

    }
}
