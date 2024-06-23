using Shared.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class RangedAttack : CoreComponent
    {
        [SerializeField] protected bool isDebug;
        [Header("Base")]
        [SerializeField] private bool isAutoAnim;
        [SerializeField] public float totalCooldownTime;
        [SerializeField] public AudioClip attackSound;

        private Timer cooldownTimer;
        public bool isDuringCooldown { get; private set; }
        public float restCooldownTime { get; private set; }

        public event Action<RangedAttackStatus> OnStatusChange;
        public RangedAttackStatus statusIndex {  get; protected set; }
        public bool allowAttackDetection {  get; protected set; }
        protected AnimationEventHandler animationEventHandler;
        private IAblity ablity;
        public enum RangedAttackStatus
        {
            Idle,
            Charge,
            Attack,
        }

        protected override void Awake()
        {
            base.Awake();

            cooldownTimer = new Timer(totalCooldownTime);
            cooldownTimer.OnTimerDone += HandleCooldownTimerDone;

            ablity = GetComponent<IAblity>();

            animationEventHandler = Core.transform.parent.GetComponent<AnimationEventHandler>();
        }

        public override void OnEnable()
        {
            animationEventHandler.OnFinish += HandleOnAttackFinished;
        }

        private void OnDisable()
        {
            animationEventHandler.OnFinish -= HandleOnAttackFinished;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            cooldownTimer.Tick();
            restCooldownTime = cooldownTimer.restTime;
        }

        public virtual void CheckIfAttack()
        {
            if (isDuringCooldown)
            {
                return;
            }

            StartAttack();
        }

        public virtual void StartAttack()
        {
            isDuringCooldown = true;

            cooldownTimer.StartTimer();

            // 直接播放攻击动画，攻击动画的事件再触发技能
            if (isAutoAnim)
            {
                SetStatus(RangedAttackStatus.Attack);
                animationEventHandler.OnAttackAction += HandleOnAttack;
            }

            // 直接施法技能， 技能内部控制动画
            else
            {
                ablity.Activate();
            }
        }
        
        private void SetStatus(RangedAttackStatus status)
        {
            if (statusIndex != status)
            {
                statusIndex = status;
                OnStatusChange?.Invoke(status);
            }
        }

        private void HandleOnAttackFinished()
        {
            SetStatus(RangedAttackStatus.Idle);
        }

        private void HandleOnAttack()
        {
            animationEventHandler.OnAttackAction -= HandleOnAttack;

            ablity.Activate();
        }

        private void HandleCooldownTimerDone()
        {
            isDuringCooldown = false;   
        }
    }
}