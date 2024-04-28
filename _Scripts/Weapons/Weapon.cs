using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Utilities;
using Ginko.CoreSystem;

namespace Ginko.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private float attackCounterResetCooldown;

        public event Action<bool> OnCurrentInputChange;
        public event Action OnExit;
        public event Action OnEnter;
        public WeaponDataSO Data { get; private set; }
        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            set => currentAttackCounter = value >= Data.NumberOfAttacks ? 0 : value;
        }

        public bool CurrentInput
        {
            get => currentInput;
            set
            {
                if (currentInput != value)
                {
                    currentInput = value;
                    OnCurrentInputChange?.Invoke(currentInput);
                }
            }
        }
        

        public GameObject BaseGameObject { get; private set; }
        public GameObject WeaponSpriteGameObject { get; private set; }
        private Animator anim;
        public AnimationEventHandler EventHandler { get; private set; }
        public Core Core { get; private set; }

        private int currentAttackCounter;
        private Timer attackCounterResetTimer;
        private bool currentInput;
        private SpriteRenderer weaponSpriteRenderer;

        private void Awake()
        {
            BaseGameObject = transform.Find("Base").gameObject;
            WeaponSpriteGameObject = transform.Find("WeaponSprite").gameObject;
            weaponSpriteRenderer = WeaponSpriteGameObject.GetComponent<SpriteRenderer>();

            anim = BaseGameObject.GetComponent<Animator>();
            EventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();


            attackCounterResetTimer = new Timer(attackCounterResetCooldown);
        }
        private void Update()
        {
            attackCounterResetTimer.Tick();
        }
        public void Enter()
        {
            attackCounterResetTimer.StopTimer();

            anim.SetBool("active", true);
            anim.SetInteger("counter", CurrentAttackCounter);

            OnEnter?.Invoke();
        }
        public void Exit()
        {
            anim.SetBool("active", false);
            CurrentAttackCounter++;
            attackCounterResetTimer.StartTimer();

            OnExit?.Invoke();
        }
        private void OnEnable()
        {
            weaponSpriteRenderer.sprite = null;

            EventHandler.OnFinish += Exit;
            attackCounterResetTimer.OnTimerDone += ResetCounter;
        }
        private void OnDisable()
        {
            // ½ÇÉ«ËÀÍöÊ±£¬ÖØÖÃ¹¥»÷¶¯»­µÄÌùÍ¼
            anim.SetBool("active", false);
            weaponSpriteRenderer.sprite = null;

            EventHandler.OnFinish -= Exit;
            attackCounterResetTimer.OnTimerDone -= ResetCounter;
        }

        private void ResetCounter()
        {
            CurrentAttackCounter = 0;
        }
        public void SetCore(Core core)
        {
            Core = core;
        }

        public void SetData(WeaponDataSO data)
        {
            Data = data;
        }
    }
}
