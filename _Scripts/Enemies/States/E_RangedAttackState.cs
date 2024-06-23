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
    /* 目前仅负责
     * 1.调用 RangedAttack
     * 2.切换至其他state
     */
    public class E_RangedAttackState : AttackState
    {
        public E_RangedAttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
        }

        // 因为具体动画由不同的Ability决定，所以默认使用Idle动画
        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.Idle;
        }

        public override void RegisterEvents()
        {
            base.RegisterEvents();
            Entity.RangedAttack.OnAnimChange += UpdateAnim;
        }

        public override void UnRegisterEvents()
        {
            base.UnRegisterEvents();
            Entity.RangedAttack.OnAnimChange -= UpdateAnim;
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

        private void UpdateAnim(AnimBoolName name)
        {
            Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), name == AnimBoolName.Idle);
            Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), name == AnimBoolName.Charge);
            Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), name == AnimBoolName.RangedAttack);
        }

        private void ClearAnim()
        {
            Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), 
                false);
            Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), 
                false);
            Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), 
                false);
        }
    }
}