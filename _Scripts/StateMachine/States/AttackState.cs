using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public abstract class AttackState : State
    {
        public AttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Entity.Movement.SetVelocityZero();
            Entity.Anim.SetInteger("AttackCounter", 0);
        }
    }
}
