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

    protected GameObject buffEffect;
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

        if (resourceBuffData?.vfx_actvieType == ResourceBuffDataSO.VFX_ActiveType.OnActivate)
        {
            vfx_timer = new Timer(resourceBuffData.vfx_activeTime);
            vfx_timer.OnTimerDone += HandleVFX_TimerDone;
        }

        if (resourceBuffData != null && resourceBuffData.resourceType == AttributeType.MaxHealth)
        {
            resourceStat = buffManager.Core.GetCoreComponent<Stats>().Health;
        }
    }

    private void OnDisable()
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

    public override void RefreshBuff()
    {
        passedTime = 0;
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

        UpdateBuffData(null);
    }

    private void CheckSwitchOnBuff_VFX()
    {
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
            if (buffEffect == null)
            {
                buffEffect = Instantiate(resourceBuffData.buffEffect, transform);
                buffEffect.transform.localPosition = resourceBuffData.vfx_offset;
                buffEffect.transform.localScale = resourceBuffData.vfx_scale;
            }
            else
            {
                buffEffect.SetActive(true);
            }

            vfx_timer?.StartTimer();
        }
        else if (buffEffect != null && buffEffect.activeSelf)
        {
            buffEffect.SetActive(false);
        }
    }

    private void HandleVFX_TimerDone()
    {
        SwitchBuff_VFX(false);
    }
}