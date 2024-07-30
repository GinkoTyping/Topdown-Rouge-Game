using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    [SerializeField] public BaseBuffDataSO data;

    [HideInInspector] public float currenrStack = 0f;
    [HideInInspector] public float buffTimer;

    protected BuffManager buffManager;
    protected BuffIcon currentBuffIcon;

    protected virtual void Start()
    {
        buffManager = GetComponentInParent<BuffManager>();
        buffManager.RegisterBuff(this);

        UpdateSpecificBuffData();
    }

    private void OnEnable()
    {
        currentBuffIcon = null;
    }

    public virtual void Init() { }

    public abstract void LogicUpdate();

    protected abstract void UpdateSpecificBuffData();

    public abstract void RefreshBuff();

    protected void SwitchBuffIcon(bool isShow)
    {
        if (isShow)
        {
            if (currentBuffIcon == null && buffManager.buffsPool.Pool != null)
            {
                GameObject buffGO = buffManager.buffsPool.Pool.Get();
                BuffIcon buffIcon = buffGO.GetComponent<BuffIcon>();
                currentBuffIcon = buffIcon;

                buffIcon.SetPool(buffManager.buffsPool);
                buffIcon.Set(this);
            }
        } else
        {
            if (currentBuffIcon != null)
            {
                currentBuffIcon.poolManager.Pool.Release(currentBuffIcon.gameObject);
            }
        }
    }

    protected void UpdateBuffData(BaseBuffDataSO newBuffData)
    {
        data = newBuffData;

        UpdateSpecificBuffData();
    }
}
