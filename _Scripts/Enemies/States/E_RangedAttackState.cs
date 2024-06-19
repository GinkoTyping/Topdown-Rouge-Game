using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.Weapons;
using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Ginko.Data;
using Shared.Utilities;
using UnityEditor;

namespace Ginko.StateMachineSystem
{
    public class E_RangedAttackState : AttackState
    {
        private Timer timer;

        private enum Status
        {
            Idle,
            Charge,
            Attack,
        }

        public E_RangedAttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }
        
        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.Charge;
        }

        public override void RegisterEvents()
        {
            base.RegisterEvents();
            animEventHandler.OnAttackAction += HandleStartAttack;
        }

        public override void UnRegisterEvents()
        {
            base.UnRegisterEvents();
            animEventHandler.OnAttackAction -= HandleStartAttack;
        }

        public override void Enter()
        {
            base.Enter();

            Entity.Movement.SetVelocityZero();
            Entity.Anim.SetInteger("AttackCounter", 0);

            SetTimer();
            Entity.RangedAttack.Set();
        }

        public override void Exit()
        {
            base.Exit();

            timer.StopTimer();
            Entity.RangedAttack.SetAllowDetection(false);
            //Entity.CollideAttack.StartDetection(false);

            Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), false);

            Entity.Anim.SetInteger("AttackCounter", -1);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Entity.RangedAttack.Attack();

            UpdateAnimByAttackStatus();

            if (timer.isActive)
            {
                timer.Tick();
            } 
            else if (IsAnimationFinished)
            {
                Entity.RangedAttack.SetAllowDetection(false);

                if (!Entity.Detections.IsInRangedAttackRange)
                {
                    timer.StopTimer();
                    UpdateAnimByAttackStatus(isClear: true);
                }

                if (Entity.Detections.IsInMeleeAttackRange)
                {
                    IsToMeleeAttackState = true;
                }
                else if (Entity.Detections.IsInRangedAttackRange)
                {
                    if (!timer.isActive)
                    {
                        IsAnimationFinished = false;
                        timer.StartTimer();

                        Entity.RangedAttack.SetStatus(RangedAttack.RangedAttackStatus.Idle);
                        UpdateAnimByAttackStatus();
                    }
                }
                else if (Entity.Detections.IsHostileDetected)
                {
                    IsToHostileDetectedState = true;
                }
                else if (!Entity.Detections.IsHostileDetected)
                {
                    IsToIdleState = true;
                }
            }
        }

        private void UpdateAnimByAttackStatus(bool isClear = false)
        {
            if (isClear)
            {
                Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), false);
                Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), false);
                Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), false);
            }
            else
            {
                Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), Entity.RangedAttack.statusIndex == RangedAttack.RangedAttackStatus.Idle);
                Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), Entity.RangedAttack.statusIndex == RangedAttack.RangedAttackStatus.Charge);
                Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), Entity.RangedAttack.statusIndex == RangedAttack.RangedAttackStatus.Attack);
            }
        }

        private void SetTimer()
        {
            timer = new Timer(1.0f);
            timer.OnTimerDone += HandleTimerDone;
        }

        private void HandleTimerDone()
        {
            Entity.RangedAttack.Set();
        }

        private void HandleStartAttack()
        {
            SoundManager.Instance.PlaySound(Entity.RangedAttack.attackSound);
        }
    }
}