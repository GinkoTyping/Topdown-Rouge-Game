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
        [SerializeField] public float totalCooldownTime;

        private Timer cooldownTimer;
        private AnimationEventHandler animationEventHandler;
        private BaseAbility ablity;

        private bool isDuringCooldown;
        private bool isToModify;
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

            cooldownTimer = null;
            cooldownTimer = new Timer(cooldownTime);
            cooldownTimer.OnTimerDone += HandleCooldownTimerDone;
        }

        public void ModifyCooldown(float cooldownTime)
        {
            if (cooldownTimer.isActive)
            {
                totalCooldownTime = cooldownTime;
                isToModify = true;

                cooldownTimer.OnTimerDone += ModifyTimerOnTimerDone;
            } else
            {
                SetCooldown(cooldownTime);
            }
        }

        public void CheckIfAttack()
        {
            if (isDuringCooldown || isToModify)
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

        private void ModifyTimerOnTimerDone()
        {
            SetCooldown(totalCooldownTime);

            cooldownTimer.OnTimerDone -= ModifyTimerOnTimerDone;
            isToModify = false;
        }
    }
}