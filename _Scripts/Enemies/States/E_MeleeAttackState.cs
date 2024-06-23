using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.Weapons;
using Ginko.CoreSystem;
using System;

namespace Ginko.StateMachineSystem
{
    public class E_MeleeAttackState : AttackState
    {
        private AnimationEventHandler animationEventHandler;
        private Attack attack;

        public E_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            animationEventHandler = entity.GetComponent<AnimationEventHandler>();
            attack = entity.Core.GetCoreComponent<Attack>();
        }

        public override void Exit()
        {
            base.Exit();

            Entity.Anim.SetInteger("AttackCounter", -1);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsAnimationFinished)
            {
                if (!Entity.Detections.IsInMeleeAttackRange 
                    && Entity.Detections.IsInRangedAttackRange)
                {
                    IsToRangedAttackState = true;
                }
                else if (!Entity.Detections.IsInMeleeAttackRange
                    && Entity.Detections.IsHostileDetected)
                {
                    IsToHostileDetectedState = true;
                }
                else if (!Entity.Detections.IsHostileDetected)
                {
                    IsToIdleState = true;
                }
            }
        }

        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.MeleeAttack;
        }

        public override void RegisterEvents()
        {
            base.RegisterEvents();

            animationEventHandler.OnFinish += AddAttackCounter;
            animationEventHandler.OnAttackAction += attack.InstaniateMeleeAttack;
        }
        
        public override void UnRegisterEvents()
        {
            base.UnRegisterEvents();

            animationEventHandler.OnFinish -= AddAttackCounter;
            animationEventHandler.OnAttackAction -= attack.InstaniateMeleeAttack;
        }

        public void AddAttackCounter()
        {
            if (Entity.Detections.IsInMeleeAttackRange)
            {
                int counter = Entity.Anim.GetInteger("AttackCounter") + 1;
                if (counter > Entity.Attack.attackDetails.Length - 1)
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