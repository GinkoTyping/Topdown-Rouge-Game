using Ginko.Data;
using Ginko.StateMachineSystem;
using Ginko.Weapons;
using System;
using UnityEngine;

namespace Ginko.EnemySystem
{
    public class Enemy : Entity
    {
        public Action OnObstaclesCollision;
        public AnimationEventHandler AnimationEventHandler;

        protected override void InitiateBasicStates()
        {
            IdleState = new E_IdleState(this, StateMachine);
            HostileDetectedState = new E_HostileDetectedState(this, StateMachine);
            AttackState = new E_AttackState(this, StateMachine);
        }

        protected override void InitiateStateMachine()
        {
            StateMachine.Initialize(IdleState);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Obstacles")
            {
                OnObstaclesCollision?.Invoke();
            }
        }
    }
}
