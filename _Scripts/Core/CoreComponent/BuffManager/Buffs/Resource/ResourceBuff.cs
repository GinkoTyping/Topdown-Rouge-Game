using Ginko.CoreSystem;
using Shared.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceBuff : Buff
{
    protected ResourceBuffDataSO resourceBuffData;
    protected ResourceStat resourceStat;

    protected float passedTime = -1;
    protected float calculateTime;

    protected GameObject buffVFX;
    protected DamageReceiver damageReceiverComp;

    protected Timer vfx_timer;

    protected override void Start()
    {
        base.Start();

        damageReceiverComp = buffManager.Core.GetCoreComponent<DamageReceiver>();

        RefreshBuff();
    }

    protected override void UpdateSpecificBuffData()
    {
        resourceBuffData = data as ResourceBuffDataSO;

        if (resourceBuffData?.buff_vfx != null && resourceBuffData?.vfx_actvieType == ResourceBuffDataSO.VFX_ActiveType.OnActivate)
        {
            vfx_timer = new Timer(resourceBuffData.vfx_activeTime);
            vfx_timer.OnTimerDone += HandleVFX_TimerDone;
        }

        if (resourceBuffData != null && resourceBuffData.resourceType == ResourceType.Health)
        {
            resourceStat = buffManager.Core.GetCoreComponent<Stats>().Health;
        }
    }

    protected virtual void OnDisable()
    {
        if (vfx_timer != null)
        {
            vfx_timer.OnTimerDone -= HandleVFX_TimerDone;
        }
    }

    public override void LogicUpdate()
    {
        UpdateVFX_Timer();

        if (resourceBuffData != null)
        {
            if (passedTime > resourceBuffData.totalTime)
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

    public override void RefreshBuff(BaseBuffDataSO newData = null)
    {
        base.RefreshBuff(newData);

        passedTime = 0;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
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
        if (resourceBuffData.buff_vfx == null)
        {
            return;
        }

        if (passedTime == 0 && resourceBuffData.vfx_actvieType == ResourceBuffDataSO.VFX_ActiveType.DuringActive)
        {
            SwitchBuff_VFX(true);
        }
        else if (calculateTime >= resourceBuffData.perTime && resourceBuffData.vfx_actvieType == ResourceBuffDataSO.VFX_ActiveType.OnActivate)
        {
            SwitchBuff_VFX(true);
        }
    }

    private void CheckSwitchOnBuffIcon()
    {
        if (passedTime == 0)
        {
            SwitchBuffIcon(true);
            buffTimer = resourceBuffData.totalTime;
        }
    }

    private void SwitchBuff_VFX(bool isShow)
    {
        if (isShow)
        {
            if (buffVFX == null)
            {
                buffVFX = Instantiate(resourceBuffData.buff_vfx, transform);
                buffVFX.transform.localPosition = resourceBuffData.vfx_offset;
                buffVFX.transform.localScale = resourceBuffData.vfx_scale;
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

    public override string GetDesc()
    {
        string moduleDesc = data.desc;

        string color;
        string statName;
        if (resourceBuffData.isAttribute)
        {
            color = attributeHelper.GetAttributeColor(resourceBuffData.attributeType);
            statName = attributeHelper.ShortenAttributeName(resourceBuffData.attributeType);
        }
        else
        {
            color = attributeHelper.GetAttributeColor(resourceBuffData.resourceType);
            statName = attributeHelper.ShortenAttributeName(resourceBuffData.resourceType);
        }

        float value = resourceBuffData.isUpdateOverTime ? resourceBuffData.perValue : resourceBuffData.staticValue;
        string valueString;
        if (resourceBuffData.isAttribute && attributeHelper.IsFloat(resourceBuffData.attributeType))
        {
            valueString = $"{value * 100}%";
        } else
        {
            valueString = value.ToString();
        }

        moduleDesc = moduleDesc.Replace("{$1}", GetSpecialText(valueString, color));
        moduleDesc = moduleDesc.Replace("{$2}", GetSpecialText(statName, color));
        moduleDesc = moduleDesc.Replace("{$3}", GetSpecialText(resourceBuffData.perTime));

        return moduleDesc;
    }
}