using Shared.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SommoningSpell : BaseAbility
{
    [Header("Sommon")]
    [SerializeField] private GameObject magicAuraPrefab;
    [SerializeField] private float auarTime;
    [SerializeField] private GameObject sommonPrefab;
    [SerializeField] private float sommonArea;

    private PoolHelper poolHelper;
    private PoolManager sommonPoolManager;
    private PoolManager auraPoolManager;

    private Transform sommonPoolParent;
    private Transform containersTransform;

    private List<GameObject> activeAuras = new List<GameObject>();
    private List<Timer> activeTimers = new List<Timer>();

    private void Awake()
    {
        poolHelper = GameObject.Find("Helper").GetComponent<PoolHelper>();
        sommonPoolParent = transform.parent.parent.parent.transform;
        containersTransform = GameObject.Find("Containers").transform;
    }

    private void Update()
    {
        if (activeAuras.Count > 0)
        {
            for (int i = 0; i < activeTimers.Count; i++)
            {
                activeTimers[i].Tick();
            }
        }
    }

    public override void Activate()
    {
        if (auraPoolManager == null)
        {
            auraPoolManager = poolHelper.GetPoolByPrefab(containersTransform, magicAuraPrefab);
        }

        GameObject auraGO = auraPoolManager.Pool.Get();
        activeAuras.Add(auraGO);

        auraGO.transform.eulerAngles = new Vector3(-30, 0, 0);
        auraGO.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-sommonArea, sommonArea), UnityEngine.Random.Range(-sommonArea, sommonArea), 0);

        Timer timer = new Timer(auarTime);
        timer.OnTimerDone += InstaniateEnemyOnAura;
        timer.StartTimer();
        activeTimers.Add(timer);
    }
    private void InstaniateEnemyOnAura()
    {
        if (sommonPoolManager == null)
        {
            sommonPoolManager = poolHelper.GetPoolByPrefab(sommonPoolParent, sommonPrefab);
        }
        GameObject sommonnedGO = sommonPoolManager.Pool.Get();
        sommonnedGO.transform.position = activeAuras[0].transform.position;

        auraPoolManager.Pool.Release(activeAuras[0]);
        activeAuras.RemoveAt(0);
        activeTimers.RemoveAt(0);
    }
}
