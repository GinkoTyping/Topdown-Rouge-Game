using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public abstract class HostileDetectedState : State
    {
        public HostileDetectedState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.HostileDetected;
        }
    }
}