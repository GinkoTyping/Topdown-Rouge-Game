using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    [SerializeField] public BaseBuffDataSO data;

    protected BuffManager buffManager;

    protected BuffIcon currentBuffIcon;
    public float currenrStack = 0f;

    protected virtual void Start()
    {
        buffManager = GetComponentInParent<BuffManager>();
        buffManager.Add(this);
    }

    public abstract void LogicUpdate();

    protected void InstantiateBuffIcon()
    {
        if (currentBuffIcon == null)
        {
            GameObject buffGO = buffManager.buffsPool.Pool.Get();
            BuffIcon buffIcon = buffGO.GetComponent<BuffIcon>();
            currentBuffIcon = buffIcon;

            buffIcon.SetPool(buffManager.buffsPool);
            buffIcon.Set(this);
        }
    }
}
