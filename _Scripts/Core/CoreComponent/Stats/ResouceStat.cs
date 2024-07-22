using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    [Serializable]
    public class ResouceStat : BaseStat
    {
        [field: SerializeField] 
        public float RecoveryRate { get; private set; }
        public void Init()
        {
            CurrentValue = MaxValue;
        }
        public void LogicUpdate()
        {
            if (RecoveryRate > 0 && CurrentValue < MaxValue)
            {
                Increase(RecoveryRate * Time.deltaTime);
            }
        }
    }

    public enum ResourceType
    {
        Health,
        Mana,
    }
}