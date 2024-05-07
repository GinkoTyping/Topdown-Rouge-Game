using Ginko.StateMachineSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    public ObjectPool<GameObject> Pool { get; private set; }

    private GameObject currentObject;
    private GameObject EnemiesHolder;

    private void Awake()
    {
        Instance = this;

        Pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, 100);
        EnemiesHolder = GameObject.Find("Enemies");
    }

    private void actionOnDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    private void actionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void actionOnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    private GameObject createFunc()
    {
        GameObject obj =  Instantiate(currentObject, Vector2.zero, Quaternion.identity, EnemiesHolder.transform);
        obj.GetComponent<Entity>().SetPool(Pool);

        return obj;
    }

    public void SetCurrrentObject(GameObject obj)
    {
        currentObject = obj;
    }
}
