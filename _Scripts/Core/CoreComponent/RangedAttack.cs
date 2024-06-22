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
        [SerializeField] protected LayerMask hostileLayer;
        [SerializeField] protected float attackDamage;
        [SerializeField] public float attackInterval;
        [SerializeField] public AudioClip attackSound;

        private Timer cooldownTimer;
        protected bool isDuringCooldown;
        public float cooldownTime;

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

            cooldownTimer = new Timer(attackInterval);
            cooldownTimer.OnTimerDone += OnCooldownTimerDone;

            ablity = GetComponent<IAblity>();

            animationEventHandler = Core.transform.parent.GetComponent<AnimationEventHandler>();
        }

        public override void OnEnable()
        {
            if (ablity != null)
            {
                ablity.OnAttack += OnAblityAttack;
            }
        }

        private void OnDisable()
        {
            if (ablity != null)
            {
                ablity.OnAttack -= OnAblityAttack;
            }
        }

        private void OnAblityAttack()
        {
            SetStatus(RangedAttackStatus.Attack);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            cooldownTimer.Tick();
            cooldownTime = cooldownTimer.restTime;
        }

        public void SetStatus(RangedAttackStatus status)
        {
            if (statusIndex != status)
            {
                statusIndex = status;
                OnStatusChange?.Invoke(status);
            }
        }

        public void SetAllowDetection(bool isAllow)
        {
            allowAttackDetection = isAllow;
        }

        public virtual void Set(bool isDefault = false)
        {

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

            if (isAutoAnim)
            {
                SetStatus(RangedAttackStatus.Attack);
                animationEventHandler.OnAttackAction += HandleOnAttack;
            }
            else
            {
                ablity.Activate();
            }
        }

        private void HandleOnAttack()
        {
            animationEventHandler.OnAttackAction -= HandleOnAttack;

            ablity.Activate();
        }
        public virtual void StopAttack()
        {
            //cooldownTimer?.StopTimer();
        }

        private void OnCooldownTimerDone()
        {
            isDuringCooldown = false;   
        }
    }
}