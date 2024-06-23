using Ginko.StateMachineSystem;
using Shared.Utilities;
using System;
using UnityEngine;

namespace Ginko.CoreSystem
{
    /*
     * 1. 根据冷却时间调用 Ability
     */
    public class RangedAttack : CoreComponent
    {
        [Header("Base")]
        [SerializeField] private bool isAutoAnim;
        [SerializeField] private float totalCooldownTime;

        private Timer cooldownTimer;
        private AnimationEventHandler animationEventHandler;
        private BaseAbility ablity;

        private bool isDuringCooldown;
        private float restCooldownTime;
        private AnimBoolName currentAnim;

        public event Action<AnimBoolName> OnAnimChange;

        #region Hooks
        protected override void Awake()
        {
            base.Awake();

            cooldownTimer = new Timer(totalCooldownTime);
            cooldownTimer.OnTimerDone += HandleCooldownTimerDone;

            ablity = GetComponent<BaseAbility>();

            animationEventHandler = Core.transform.parent.GetComponent<AnimationEventHandler>();
        }

        public override void OnEnable()
        {
            animationEventHandler.OnAttackAction += PlayAttackSound;
            animationEventHandler.OnFinish += HandleOnAttackFinished;
        }

        private void OnDisable()
        {
            animationEventHandler.OnAttackAction -= PlayAttackSound;
            animationEventHandler.OnFinish -= HandleOnAttackFinished;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            cooldownTimer.Tick();
            restCooldownTime = cooldownTimer.restTime;
        }
        #endregion

        public virtual void CheckIfAttack()
        {
            if (isDuringCooldown)
            {
                return;
            }

            StartAttack();
        }

        private void StartAttack()
        {
            isDuringCooldown = true;

            cooldownTimer.StartTimer();

            // 直接播放攻击动画，攻击动画的事件再触发技能
            if (isAutoAnim)
            {
                SetAnimState(AnimBoolName.RangedAttack);
                animationEventHandler.OnAttackAction += HandleOnAttack;
            }

            // 直接施法技能， 技能内部控制动画
            else
            {
                ablity.Activate();
            }
        }
        
        private void SetAnimState(AnimBoolName name)
        {
            if (currentAnim != name)
            {
                currentAnim = name;
                OnAnimChange?.Invoke(name);
            }
        }

        private void HandleOnAttackFinished()
        {
            SetAnimState(AnimBoolName.Idle);
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

        private void PlayAttackSound()
        {
            SoundManager.Instance.PlaySound(ablity.abilityAudio);
        }
    }
}