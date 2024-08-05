using Ginko.CoreSystem;
using Shared.Utilities;
using UnityEngine;

public abstract class ResourceBuff : Buff
{
    protected StatusBuffDataSO statusBuffData;
    protected BaseStat stat;

    protected float passedTime;
    protected float calculateTime;

    protected GameObject buffVFX;
    protected DamageReceiver damageReceiverComp;

    protected Timer vfx_timer;

    protected override void Start()
    {
        base.Start();

        damageReceiverComp = buffManager.Core.GetCoreComponent<DamageReceiver>();

        if (statusBuffData.isAttribute)
        {
            stat = buffManager.Core.GetCoreComponent<Stats>().GetAttribute(statusBuffData.attributeType);
        } else
        {
            stat = buffManager.Core.GetCoreComponent<Stats>().GetAttribute(statusBuffData.resourceType);
        }
    }

    protected override void UpdateSpecificBuffData()
    {
        statusBuffData = data as StatusBuffDataSO;

        if (statusBuffData?.buff_vfx != null && statusBuffData?.vfx_actvieType == StatusBuffDataSO.VFX_ActiveType.OnActivate)
        {
            vfx_timer = new Timer(statusBuffData.vfx_activeTime);
            vfx_timer.OnTimerDone += HandleVFX_TimerDone;
        }
    }

    protected virtual void OnDisable()
    {
        passedTime = 0;

        if (vfx_timer != null)
        {
            vfx_timer.OnTimerDone -= HandleVFX_TimerDone;
        }
    }

    public override void LogicUpdate()
    {
        UpdateVFX_Timer();

        if (statusBuffData != null)
        {
            if (passedTime > statusBuffData.totalTime)
            {
                HandleBuffOver();
            }
            else
            {
                CheckSwitchOnBuffIcon();
                CheckSwitchOnBuff_VFX();

                ApplyBuffEffect();

                UpdateTimeConfig();
            }
        }
    }

    protected abstract void ApplyBuffEffect();

    private void UpdateVFX_Timer()
    {
        if (vfx_timer != null)
        {
            vfx_timer.Tick();
        }
    }

    private void UpdateTimeConfig()
    {
        passedTime += Time.deltaTime;
        buffTimer -= Time.deltaTime;
    }

    private void HandleBuffOver()
    {
        calculateTime = 0;

        SwitchBuff_VFX(false);
        SwitchBuffIcon(false);

        // OnDisabled 需要 buffData 数据，所以先Disabled再重置 buffData
        gameObject.SetActive(false);
        UpdateBuffData(null);
    }

    private void CheckSwitchOnBuff_VFX()
    {
        if (statusBuffData.buff_vfx == null)
        {
            return;
        }

        if (passedTime == 0 && statusBuffData.vfx_actvieType == StatusBuffDataSO.VFX_ActiveType.DuringActive)
        {
            SwitchBuff_VFX(true);
        }
        else if (calculateTime >= statusBuffData.perTime && statusBuffData.vfx_actvieType == StatusBuffDataSO.VFX_ActiveType.OnActivate)
        {
            SwitchBuff_VFX(true);
        }
    }

    private void CheckSwitchOnBuffIcon()
    {
        if (passedTime == 0)
        {
            SwitchBuffIcon(true);
            buffTimer = statusBuffData.totalTime;
        }
    }

    private void SwitchBuff_VFX(bool isShow)
    {
        if (isShow)
        {
            if (buffVFX == null)
            {
                buffVFX = Instantiate(statusBuffData.buff_vfx, transform);
                buffVFX.transform.localPosition = statusBuffData.vfx_offset;
                buffVFX.transform.localScale = statusBuffData.vfx_scale;
            }
            else
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

    private void HandleVFX_TimerDone()
    {
        SwitchBuff_VFX(false);
    }

    public override string GetDesc(bool hasDurationText = false)
    {
        base.GetDesc(hasDurationText);

        string moduleDesc = data.desc;

        string color;
        string statName;
        if (statusBuffData.isAttribute)
        {
            color = attributeHelper.GetAttributeColor(statusBuffData.attributeType);
            statName = attributeHelper.ShortenAttributeName(statusBuffData.attributeType);
        }
        else
        {
            color = attributeHelper.GetAttributeColor(statusBuffData.resourceType);
            statName = attributeHelper.ShortenAttributeName(statusBuffData.resourceType);
        }

        float value = statusBuffData.isUpdateOverTime ? statusBuffData.perValue : statusBuffData.staticValue;
        string valueString;
        if (statusBuffData.isAttribute && attributeHelper.IsFloat(statusBuffData.attributeType))
        {
            valueString = $"{value * 100}%";
        } else
        {
            valueString = value.ToString();
        }

        moduleDesc = moduleDesc.Replace("{$1}", GetSpecialText(valueString, color));
        moduleDesc = moduleDesc.Replace("{$2}", GetSpecialText(statName, color));
        moduleDesc = moduleDesc.Replace("{$3}", GetSpecialText(statusBuffData.perTime));

        if (hasDurationText)
        {
            moduleDesc += $" ,last {GetSpecialText(statusBuffData.totalTime)} seconds";
        }

        return moduleDesc;
    }
}