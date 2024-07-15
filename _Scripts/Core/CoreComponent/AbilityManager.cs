using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using Shared.Utilities;
using System;
using UnityEngine;

namespace Ginko.CoreSystem
{
    /*
     * 1. ������ȴʱ����� Ability
     */
    public class AbilityManager : CoreComponent
    {
        [Header("Base")]
        [SerializeField] private bool isAutoAnim;
        [SerializeField] private AnimBoolName defaultAttackAnimBoolName;
        [SerializeField] private float totalCooldownTime;

        private Timer cooldownTimer;
        private AnimationEventHandler animationEventHandler;
        private BaseAbility ablity;

        private bool isDuringCooldown;
        private float restCooldownTime;
        private Animator animator;

        public event Action<AnimBoolName> OnAnimChange;

        #region Hooks
        protected override void Awake()
        {
            base.Awake();

            cooldownTimer = new Timer(totalCooldownTime);
            cooldownTimer.OnTimerDone += HandleCooldownTimerDone;

            ablity = GetComponent<BaseAbility>();

            animationEventHandler = Core.transform.parent.GetComponent<AnimationEventHandler>();
            animator = Core.GetComponentInParent<Animator>();
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
        #endregion

        public void SetCooldown(float cooldownTime)
        {
            totalCooldownTime = cooldownTime;

            cooldownTimer = new Timer(cooldownTime);
            cooldownTimer.OnTimerDone += HandleCooldownTimerDone;
        }

        public void CheckIfAttack()
        {
            if (isDuringCooldown)
            {
                return;
            }

            InitiateAttack();
        }

        private void InitiateAttack()
        {
            isDuringCooldown = true;

            cooldownTimer.StartTimer();

            // ֱ�Ӳ��Ź��������������������¼��ٴ�������
            if (isAutoAnim)
            {
                SetAnimState(defaultAttackAnimBoolName);
                animationEventHandler.OnAttackAction += HandleOnAttack;
            }

            // ֱ��ʩ�����ܣ� �����ڲ����ƶ���
            else
            {
                ablity.Activate();
            }
        }
        
        private void SetAnimState(AnimBoolName name)
        {
            if (!animator
                    .GetCurrentAnimatorStateInfo(0)
                    .IsName(name.ToString())
                )
            {
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
    }
}