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
        [SerializeField] private GameObject magicAuraPrefab;
        [SerializeField] private float auarTime;
        [SerializeField] private GameObject sommonPrefab;
        [SerializeField] private float sommonArea;

        private PoolHelper poolHelper;
        private PoolManager sommonPoolManager;
        private PoolManager auraPoolManager;

        private Transform entityTransform;
        private Transform containersTransform;

        private List<GameObject> activeAuras = new List<GameObject>();
        private List<Timer> activeTimers = new List<Timer>();

        protected override void Awake()
        {
            base.Awake();

            poolHelper = GameObject.Find("Helper").GetComponent<PoolHelper>();
            entityTransform = Core.transform.parent.parent;
            containersTransform = GameObject.Find("Containers").transform;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (activeAuras.Count > 0)
            {
                for (int i = 0; i < activeTimers.Count; i++)
                {
                    activeTimers[i].Tick();
                }
            }
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

            if (auraPoolManager == null)
            {
                auraPoolManager = poolHelper.GetPoolByPrefab(containersTransform, magicAuraPrefab);
            }

            GameObject auraGO = auraPoolManager.Pool.Get();
            activeAuras.Add(auraGO);

            auraGO.transform.eulerAngles = new Vector3 (-30, 0, 0);
            auraGO.transform.position = transform.position + new Vector3(Random.Range(-sommonArea, sommonArea), Random.Range(-sommonArea, sommonArea), 0);

            Timer timer = new Timer(auarTime);
            timer.OnTimerDone += InstaniateEnemyOnAura;
            timer.StartTimer();
            activeTimers.Add(timer);
        }

        private void InstaniateEnemyOnAura()
        {
            if (sommonPoolManager == null)
            {
                sommonPoolManager = poolHelper.GetPoolByPrefab(entityTransform, sommonPrefab);
            }
            GameObject sommonnedGO = sommonPoolManager.Pool.Get();
            sommonnedGO.transform.position = activeAuras[0].transform.position;

            auraPoolManager.Pool.Release(activeAuras[0]);
            activeAuras.RemoveAt(0);
            activeTimers.RemoveAt(0);
        }
    }


}