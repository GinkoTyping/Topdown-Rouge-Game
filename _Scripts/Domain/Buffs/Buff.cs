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
    protected AttributeHelper attributeHelper;

    private void Awake()
    {
        SetAttributeHelper();
        buffManager = GetComponentInParent<BuffManager>();
    }

    protected virtual void Start()
    {
        UpdateSpecificBuffData();
    }

    protected virtual void OnEnable()
    {
        currentBuffIcon = null;
    }

    public virtual void Init() 
    {
    }

    private void SetAttributeHelper()
    {
        attributeHelper = GameObject.Find("Helper").GetComponent<AttributeHelper>();
    }

    public virtual string GetDesc(bool hasDurationText = false)
    {
        if (attributeHelper == null)
        {
            SetAttributeHelper();
        }

        return "";
    }

    public abstract void LogicUpdate();

    protected abstract void UpdateSpecificBuffData();

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

    public void UpdateBuffData(BaseBuffDataSO newBuffData)
    {
        data = newBuffData;

        UpdateSpecificBuffData();

        // 去（除）激活的buff更新数据之后重新触发激活；
        if (newBuffData != null && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    protected string GetSpecialText<T>(T text, string color = "#ff0000", bool underline = false)
    {
        string output = $"<size=+6><{color}>{text}</color></size>";

        return underline ? $"<u>{output}</u>" : output;
    }
}
