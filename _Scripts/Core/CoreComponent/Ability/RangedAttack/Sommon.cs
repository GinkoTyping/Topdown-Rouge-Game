using Shared.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Sommon : RangedAttack
    {
        [Header("Sommon")]
        [SerializeField] private GameObject sommonPrefab;
        [SerializeField] private float sommonArea;

        private PoolHelper poolHelper;
        private PoolManager poolManager;

        private Transform entityTransform;

        protected override void Awake()
        {
            base.Awake();

            poolHelper = GameObject.Find("Helper").GetComponent<PoolHelper>();
            entityTransform = Core.transform.parent.parent;
        }

        public override void Attack()
        {
            if (allowAttackDetection)
            {
                SetAllowDetection(false);
            }
        }

        public override void Set(bool isDefault = false)
        {
            SetStatus(RangedAttackStatus.Attack);
            animationEventHandler.OnAttackAction += SommonEnemy;
        }

        private void SommonEnemy()
        {
            animationEventHandler.OnAttackAction -= SommonEnemy;

            if (poolManager == null)
            {
                poolManager = poolHelper.GetPoolByPrefab(entityTransform, sommonPrefab);
            }

            GameObject sommonnedGO = poolManager.Pool.Get();

            sommonnedGO.transform.position = transform.position + new Vector3(Random.Range(-sommonArea, sommonArea), Random.Range(-sommonArea, sommonArea), 0);
        }

    }
}