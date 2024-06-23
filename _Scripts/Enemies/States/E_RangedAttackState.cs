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
            AnimBoolName = AnimBoolName.Idle;
        }

        public override void RegisterEvents()
        {
            base.RegisterEvents();

            animEventHandler.OnAttackAction += HandleStartAttack;
            Entity.RangedAttack.OnStatusChange += UpdateAnim;
        }

        public override void UnRegisterEvents()
        {
            base.UnRegisterEvents();

            animEventHandler.OnAttackAction -= HandleStartAttack;
            Entity.RangedAttack.OnStatusChange -= UpdateAnim;
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

            Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), false);

            Entity.Anim.SetInteger("AttackCounter", -1);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Entity.RangedAttack.CheckIfAttack();

            if (IsAnimationFinished)
            {
                if (!Entity.Detections.IsInRangedAttackRange)
                {
                    ClearAnim();
                }

                if (Entity.Detections.IsInMeleeAttackRange)
                {
                    IsToMeleeAttackState = true;
                }
                else if (Entity.Detections.IsInRangedAttackRange)
                {

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

        private void UpdateAnim(RangedAttack.RangedAttackStatus status)
        {
            if (status != RangedAttack.RangedAttackStatus.Idle)
            {
                IsAnimationFinished = false;
            }

            Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), status == RangedAttack.RangedAttackStatus.Idle);
            Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), status == RangedAttack.RangedAttackStatus.Charge);
            Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), status == RangedAttack.RangedAttackStatus.Attack);
        }

        private void ClearAnim()
        {
            Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), false);
        }

        private void HandleStartAttack()
        {
            SoundManager.Instance.PlaySound(Entity.RangedAttack.attackSound);
        }
    }
}