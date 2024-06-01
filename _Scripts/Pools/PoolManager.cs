using Ginko.StateMachineSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolManager : MonoBehaviour
{
    [SerializeField]
    private Transform containerTransform;
    [SerializeField]
    private int maxSize;

    public ObjectPool<GameObject> Pool { get; private set; }

    private GameObject currentObject;

    private void Awake()
    {
        Pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, maxSize);
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
        GameObject obj =  Instantiate(currentObject, Vector2.zero, Quaternion.identity, containerTransform);

        SetPoolReference(obj);

        return obj;
    }

    public void SetCurrrentObject(GameObject obj)
    {
        currentObject = obj;
    }

    public abstract void SetPoolReference(GameObject obj);
}
