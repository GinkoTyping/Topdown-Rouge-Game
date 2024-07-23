using Ginko.StateMachineSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ginko.CoreSystem
{
    [Serializable]
    public class ResouceStat : BaseStat
    {
        [SerializeField] private AttributeType attributeType;
        public List<ResourceChangeData> resourceChangeDatas;

        private Stats statsComp;
        private GameObject healingEffect;

        public void Init(Stats statsComp)
        {
            this.statsComp = statsComp;

            CurrentValue = InitValue == 0 ? MaxValue : InitValue;
        }

        public override void Increase(float amount)
        {
            base.Increase(amount);

            
        }

        public override void Decrease(float amount)
        {
            base.Decrease(amount);


        }

        // TODO: 迁移到BUFFMANAGER，新增BUFF图标适配
        public void LogicUpdate()
        {
            if (resourceChangeDatas.Count > 0)
            {

                resourceChangeDatas = resourceChangeDatas.Where(data => data.totalTime > 0).ToList();

                foreach (ResourceChangeData data in resourceChangeDatas)
                {
                    if (data.passedTime > data.totalTime)
                    {
                        data.totalTime = 0;
                        data.passedTime = 0;
                        data.calculateTime = 0;

                        SwitchBuffEffect(false);
                    } else
                    {
                        if (data.passedTime == 0)
                        {
                            SwitchBuffEffect(true);
                        }

                        if (data.calculateTime >= data.perTime)
                        {
                            data.calculateTime -= data.perTime;
                            Increase(data.perValue);
                        }

                        data.passedTime += Time.deltaTime;
                        data.calculateTime += Time.deltaTime;
                    }
                }
            }
        }
        private void SwitchBuffEffect(bool isShow)
        {
            if (attributeType == AttributeType.MaxHealth)
            {
                if (isShow)
                {
                    healingEffect = SharedPrefabInstantiator
                    .Instance
                    .InstantiateHealingParticle(statsComp.transform, "Healing Effect");
                } else if (healingEffect != null && healingEffect.activeSelf)
                {
                    healingEffect.SetActive(false);
                }
            }
        }
    }



    [Serializable]
    public class ResourceChangeData
    {
        [SerializeField] public float perValue;
        [SerializeField] public float perTime;
        [SerializeField] public float totalTime;
        public float passedTime;
        public float calculateTime;

        public ResourceChangeData(float value, float time, float totalTime)
        {
            perValue = value; 
            perTime = time;
            this.totalTime = totalTime;

            passedTime = 0;
            calculateTime = 0;
        }
    }

    public enum ResourceType
    {
        Health,
        Mana,
    }
}