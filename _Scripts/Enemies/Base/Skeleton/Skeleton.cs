using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.EnemySystem
{
    public class Skeleton : Enemy
    {
        protected override void InitiateBasicStates()
        {
            IdleState = new E_IdleState(this, StateMachine);
            HostileDetectedState = new E_HostileDetectedState(this, StateMachine);
            MeleeAttackState = new E_MeleeAttackState(this, StateMachine);
        }

        protected override void InitiateStateMachine()
        {
            StateMachine.Initialize(IdleState);
        }
    }
}