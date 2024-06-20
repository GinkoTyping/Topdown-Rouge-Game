using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class FireMissiles : RangedAttack
    {
        [Header("Details")]
        [SerializeField]
        private float fireDuaration;
        [SerializeField]
        private float fireVelocity;
        [SerializeField]
        private Vector3 startOffset;

        private Vector3 startPosition;
        private Vector3 fireDirection;

        private CommonPool poolManager;

        private Movement movement;

        protected override void Awake()
        {
            base.Awake();

            poolManager = GameObject.Find("Containers").transform.Find("AnimatedProjectiles").GetComponent<CommonPool>();
        }

        private void Start()
        {
            movement = Core.GetCoreComponent<Movement>();
        }

        public override void Attack()
        {
            if (allowAttackDetection)
            {
                SetAllowDetection(false);
                movement.FaceToItem(Player.Instance.transform);

                startPosition = transform.position + startOffset * movement.FacingDirection;
                fireDirection = Player.Instance.transform.position - startPosition;

                Projectile projectile = poolManager.Pool.Get().GetComponent<Projectile>();

                projectile.SetPool(poolManager);
                projectile.Set(startPosition, fireDirection, attackDamage);
                projectile.Fire(fireVelocity, fireDuaration);
            }
        }

        public override void Set(bool isDefault = false)
        {
            SetStatus(RangedAttackStatus.Attack);

            animationEventHandler.OnAttackAction += HandleAttackAction;
        }

        private void HandleAttackAction()
        {
            SetAllowDetection(true);
            animationEventHandler.OnAttackAction -= HandleAttackAction;
        }

        private void OnDrawGizmos()
        {
            if (isDebug)
            {
                Gizmos.DrawLine(transform.position + startOffset, transform.position + startOffset + Vector3.up);
            }
        }
    }
}