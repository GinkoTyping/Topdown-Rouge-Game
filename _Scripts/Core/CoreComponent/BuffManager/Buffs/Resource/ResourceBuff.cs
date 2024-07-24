using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuff : Buff
{
    private ResourceBuffDataSO resourceBuffData;
    private ResourceStat resourceStat;

    private float passedTime;
    private float calculateTime;

    private GameObject buffEffect;

    protected override void UpdateSpecificBuffData()
    {
        resourceBuffData = data as ResourceBuffDataSO;

        if (resourceBuffData != null && resourceBuffData.resourceType == AttributeType.MaxHealth)
        {
            resourceStat = buffManager.Core.GetCoreComponent<Stats>().Health;
        }
    }

    public override void LogicUpdate()
    {
        if (resourceBuffData != null)
        {
            if (passedTime > resourceBuffData.totalTime)
            {
                passedTime = 0;
                calculateTime = 0;

                SwitchBuffEffect(false);
                SwitchBuffIcon(false);

                UpdateBuffData(null);
            }
            else
            {
                if (passedTime == 0)
                {
                    SwitchBuffEffect(true);
                    SwitchBuffIcon(true);

                    buffTimer = resourceBuffData.totalTime;
                }

                if (calculateTime >= resourceBuffData.perTime)
                {
                    calculateTime -= resourceBuffData.perTime;
                    resourceStat.Increase(resourceBuffData.perValue);
                }

                passedTime += Time.deltaTime;
                calculateTime += Time.deltaTime;
                buffTimer -= Time.deltaTime;
            }
        }
    }

    private void SwitchBuffEffect(bool isShow)
    {
        if (resourceBuffData.resourceType == AttributeType.MaxHealth)
        {
            if (isShow)
            {
                if (buffEffect == null)
                {
                    buffEffect = Instantiate(resourceBuffData.buffEffect, transform);
                }
                else
                {
                    buffEffect.SetActive(true);
                }
            }
            else if (buffEffect != null && buffEffect.activeSelf)
            {
                buffEffect.SetActive(false);
            }
        }
    }
}