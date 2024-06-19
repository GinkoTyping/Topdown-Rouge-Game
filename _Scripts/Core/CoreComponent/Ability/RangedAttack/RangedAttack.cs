using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public abstract class RangedAttack : CoreComponent
    {
        [SerializeField]
        protected float attackDamage;
        [SerializeField]
        public AudioClip attackSound;
        [SerializeField]
        protected LayerMask hostileLayer;
        [SerializeField]
        protected bool isDebug;

        public RangedAttackStatus statusIndex {  get; protected set; }
        public bool allowCollideAttack {  get; protected set; }

        public enum RangedAttackStatus
        {
            Idle,
            Charge,
            Attack,
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
            allowCollideAttack = isAllow;
        }

        public abstract void Set(bool isDefault = false);

        public abstract void Attack();
    }
}