using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public abstract class RangedAttack : CoreComponent
    {
        [SerializeField] protected bool isDebug;
        [Header("Base")]
        [SerializeField] protected LayerMask hostileLayer;
        [SerializeField] protected float attackDamage;
        [SerializeField] public float attackInterval;
        [SerializeField] public AudioClip attackSound;

        public RangedAttackStatus statusIndex {  get; protected set; }
        public bool allowAttackDetection {  get; protected set; }
        protected AnimationEventHandler animationEventHandler;
        public enum RangedAttackStatus
        {
            Idle,
            Charge,
            Attack,
        }

        protected override void Awake()
        {
            base.Awake();

            animationEventHandler = Core.transform.parent.GetComponent<AnimationEventHandler>();
        }

        public void SetStatus(RangedAttackStatus status)
        {
            if (statusIndex != status)
            {
                statusIndex = status;
            }
        }

        public void SetAllowDetection(bool isAllow)
        {
            allowAttackDetection = isAllow;
        }

        public abstract void Set(bool isDefault = false);

        public abstract void Attack();
    }
}