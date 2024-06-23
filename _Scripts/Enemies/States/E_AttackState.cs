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
    public class E_AttackState : AttackState
    {
        private AbilityManager abilityManager;
        public E_AttackState(Entity entity, FiniteStateMachine stateMachine, AttackStateType attackStateType) : base(entity, stateMachine)
        {
            abilityManager = Entity.Core.transform.Find(attackStateType.ToString())?.GetComponent<AbilityManager>();
        }

        // 因为具体动画由不同的Ability决定，所以默认使用Idle动画
        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.Idle;
        }

        public override void RegisterEvents()
        {
            base.RegisterEvents();
            abilityManager.OnAnimChange += UpdateAnim;
        }

        public override void UnRegisterEvents()
        {
            base.UnRegisterEvents();
            abilityManager.OnAnimChange -= UpdateAnim;
        }

        public override void Exit()
        {
            base.Exit();

            ClearAnim();
            Entity.Anim.SetInteger("AttackCounter", -1);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            abilityManager.CheckIfAttack();

            if (IsAnimationFinished)
            {
                if (Entity.Detections.IsInMeleeAttackRange)
                {
                    IsToMeleeAttackState = true;
                }
                else if (Entity.Detections.IsInRangedAttackRange)
                {
                    IsToRangedAttackState = true;
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
            Entity.Anim.SetBool(AnimBoolName.MeleeAttack.ToString(),
                false);
            Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), 
                false);
        }
    }
    
    public enum AttackStateType
    {
        Attack,
        RangedAttack
    }
}