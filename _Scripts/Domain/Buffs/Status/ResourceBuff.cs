using Ginko.CoreSystem;
using Shared.Utilities;
using UnityEngine;

public abstract class ResourceBuff : Buff
{
    protected StatusBuffDataSO statusBuffData;
    protected BaseStat stat;

    protected float passedTime;
    protected float calculateTime;

    protected DamageReceiver damageReceiverComp;

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

        passedTime = 0;

        if (statusBuffData?.buff_vfx != null && statusBuffData?.vfx_actvieType == StatusBuffDataSO.VFX_ActiveType.OnActivate)
        {
            vfx_timer = new Timer(statusBuffData.vfx_activeTime);
            vfx_timer.OnTimerDone += HandleVFX_TimerDone;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        passedTime = 0;

        if (vfx_timer != null)
        {
            vfx_timer.OnTimerDone -= HandleVFX_TimerDone;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

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

    private void UpdateTimeConfig()
    {
        passedTime += Time.deltaTime;
        buffTimer -= Time.deltaTime;
    }

    private void HandleBuffOver()
    {
        calculateTime = 0;

        SwitchBuff_VFX(false);

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

            calculateTime = 0;
            buffTimer = statusBuffData.totalTime;
        }
    }

    private void HandleVFX_TimerDone()
    {
        SwitchBuff_VFX(false);
    }

    public override string GetDesc(bool hasDurationText = false, BaseBuffDataSO specificData = null)
    {
        base.GetDesc(hasDurationText);

        string moduleDesc = data.desc;
        StatusBuffDataSO dataToUse = statusBuffData;

        if (specificData != null)
        {
            dataToUse = specificData as StatusBuffDataSO;
        }

        string color;
        string statName;
        if (dataToUse.isAttribute)
        {
            color = attributeHelper.GetAttributeColor(dataToUse.attributeType);
            statName = attributeHelper.ShortenAttributeName(dataToUse.attributeType);
        }
        else
        {
            color = attributeHelper.GetAttributeColor(dataToUse.resourceType);
            statName = attributeHelper.ShortenAttributeName(dataToUse.resourceType);
        }

        float value = dataToUse.isUpdateOverTime ? dataToUse.perValue : dataToUse.staticValue;
        string valueString;
        if (dataToUse.isAttribute && attributeHelper.IsFloat(dataToUse.attributeType))
        {
            valueString = $"{value * 100}%";
        } else
        {
            valueString = value.ToString();
        }

        moduleDesc = moduleDesc.Replace("{$1}", GetSpecialText(valueString, color));
        moduleDesc = moduleDesc.Replace("{$2}", GetSpecialText(statName, color));
        moduleDesc = moduleDesc.Replace("{$3}", GetSpecialText(dataToUse.perTime));

        if (hasDurationText)
        {
            moduleDesc += $" ,last {GetSpecialText(dataToUse.totalTime)} seconds";
        }

        return moduleDesc;
    }
}