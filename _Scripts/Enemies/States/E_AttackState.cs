using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.Weapons;
using Ginko.CoreSystem;

namespace Ginko.StateMachineSystem
{
    public class E_AttackState : AttackState
    {
        private AnimationEventHandler animationEventHandler;
        private Attack attack;

        public E_AttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            animationEventHandler = entity.GetComponent<AnimationEventHandler>();
            attack = entity.Core.GetCoreComponent<Attack>();
        }

        public override void Enter()
        {
            base.Enter();

            Entity.Movement.SetVelocityZero();
            Entity.Anim.SetInteger("AttackCounter", 0);
        }

        public override void Exit()
        {
            base.Exit();

            Entity.Anim.SetInteger("AttackCounter", -1);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!Entity.Detections.IsInMeleeAttackRange
                && Entity.Detections.IsHostileDetected
                && IsAnimationFinished)
            {
                IsToHostileDetectedState = true;
            }
            else if (!Entity.Detections.IsHostileDetected)
            {
                IsToIdleState = true;
            }
        }

        public override void RegisterEvents()
        {
            base.RegisterEvents();

            animationEventHandler.OnFinish += AddAttackCounter;
            animationEventHandler.OnAttackAction += attack.InstaniateAttack;
        }
        public override void UnRegisterEvents()
        {
            base.UnRegisterEvents();

            animationEventHandler.OnFinish -= AddAttackCounter;
            animationEventHandler.OnAttackAction -= attack.InstaniateAttack;
        }

        public void AddAttackCounter()
        {
            if (Entity.Detections.IsInMeleeAttackRange)
            {
                int counter = Entity.Anim.GetInteger("AttackCounter") + 1;
                if (counter > 1)
                {
                    counter = 0;
                }
                Entity.Anim.SetInteger("AttackCounter", counter);
                IsAnimationFinished = false;
            }
            else
            {
                IsAnimationFinished = true;
            }
        }
    }
}