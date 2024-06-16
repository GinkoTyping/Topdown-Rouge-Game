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
        private Vector3 dashDir;
        private float dashStart;

        private Status statusIndex;

        private Timer timer;
        private EnemyBasicDataSO data;

        private enum Status
        {
            Idle,
            Charge,
            Attack,
        }

        public E_RangedAttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            data = (EnemyBasicDataSO)entity.EntityData;
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
            SetDashSetting();
        }

        public override void Exit()
        {
            base.Exit();

            timer.StopTimer();
            Entity.CollideAttack.StartDetection(false);

            Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), false);

            Entity.Anim.SetInteger("AttackCounter", -1);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            DashAttack();

            UpdateAnim();

            if (timer.isActive)
            {
                timer.Tick();
            } 
            else if (IsAnimationFinished)
            {
                Entity.CollideAttack.StartDetection(false);

                if (!Entity.Detections.IsInRangedAttackRange)
                {
                    timer.StopTimer();
                    UpdateAnim(isClear: true);
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

                        SetStatus(Status.Idle);
                        UpdateAnim();
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

        private void DashAttack()
        {
            if (dashStart != 0)
            {
                if (Time.time < dashStart + data.attackChargeTime) 
                {
                    SetStatus(Status.Charge);
                    Entity.Movement.FaceToItem(Player.Instance.transform);
                }
                else if (Time.time >= dashStart + data.attackChargeTime 
                    && Time.time < dashStart + data.attackChargeTime + data.dashDuaration)
                {
                    SetStatus(Status.Attack);
                    Entity.Movement.SetVelocity(Entity.EntityData.dashVelocity, (Vector2)dashDir);
                }
                else
                {
                    SetDashSetting(isDefault: true);
                }
            }
        }

        private void SetStatus(Status status)
        {
            if (statusIndex != status)
            {
                statusIndex = status;
            }
        }

        private void UpdateAnim(bool isClear = false)
        {
            if (isClear)
            {
                Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), false);
                Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), false);
                Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), false);
            }
            else
            {
                Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), statusIndex == Status.Idle);
                Entity.Anim.SetBool(AnimBoolName.Charge.ToString(), statusIndex == Status.Charge);
                Entity.Anim.SetBool(AnimBoolName.RangedAttack.ToString(), statusIndex == Status.Attack);
            }
        }

        private void SetDashSetting(bool isDefault = false)
        {
            if (isDefault)
            {
                Entity.Movement.SetVelocityZero();
                dashStart = 0;
                dashDir = Vector3.zero;
            } else
            {
                Entity.CollideAttack.StartDetection(true);
                Entity.SpriteHandler.TintSprite(Entity.SpriteHandler.warningColor, 3);

                Vector3 playerPos = Player.Instance.transform.position;
                dashDir = playerPos - Entity.transform.position;
                dashStart = Time.time;

                statusIndex = Status.Idle;
            }
        }

        private void SetTimer()
        {
            timer = new Timer(1.0f);
            timer.OnTimerDone += HandleTimerDone;
        }

        private void HandleTimerDone()
        {
            SetDashSetting();
        }

        private void HandleStartAttack()
        {
            SoundManager.Instance.PlaySound(Entity.CollideAttack.attackSound);
        }
    }
}