using Shared.Utilities;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    [SerializeField] public BaseBuffDataSO data;

    [HideInInspector] public float currentStack = 0f;
    [HideInInspector] public float buffTimer;

    protected BuffManager buffManager;
    protected BuffIcon currentBuffIcon;
    protected AttributeHelper attributeHelper;
    protected float startTime;

    protected GameObject buffVFX;
    protected Timer vfx_timer;

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

        startTime = Time.time;
    }

    protected virtual void OnDisable()
    {
        SwitchBuffIcon(false);
    }

    public virtual void Init() 
    {

    }

    private void SetAttributeHelper()
    {
        attributeHelper = GameObject.Find("Helper").GetComponent<AttributeHelper>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hasDurationText">展示物品详情时，需要展示时间；展示Buff图标时，不需要展示时间</param>
    /// <param name="specificData">用于在Buff对象实例化前，调用该方法的场景</param>
    /// <returns></returns>
    public virtual string GetDesc(bool hasDurationText = false, BaseBuffDataSO specificData = null)
    {
        if (attributeHelper == null)
        {
            SetAttributeHelper();
        }

        return "";
    }

    public virtual void LogicUpdate()
    {
        UpdateVFX_Timer();
    }

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

    protected void SwitchBuff_VFX(bool isShow)
    {
        if (isShow)
        {
            if (buffVFX == null)
            {
                buffVFX = Instantiate(data.buff_vfx, transform);
                buffVFX.transform.localPosition = data.vfx_offset;
                buffVFX.transform.localScale = data.vfx_scale;
            }
            else if (!buffVFX.activeSelf)
            {
                buffVFX.SetActive(true);
            }

            vfx_timer?.StartTimer();
        }
        else if (buffVFX != null && buffVFX.activeSelf)
        {
            buffVFX.SetActive(false);
        }
    }
    
    private void UpdateVFX_Timer()
    {
        if (vfx_timer != null)
        {
            vfx_timer.Tick();
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
