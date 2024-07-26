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
    protected Transform containerTransform;
    [SerializeField]
    protected GameObject defaultObject;
    [SerializeField]
    protected int maxSize;

    public ObjectPool<GameObject> Pool { get; private set; }

    protected GameObject currentObject;

    private void Awake()
    {
        Pool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, maxSize);

        currentObject = defaultObject;
        if (containerTransform == null)
        {
            containerTransform = transform;
        }
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

    public void SetCurrentParrent(Transform transform)
    {
        containerTransform = transform;
    }

    public abstract void SetPoolReference(GameObject obj);
}
