using Ginko.StateMachineSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ginko.CoreSystem
{
    [Serializable]
    public class ResourceStat : BaseStat
    {
        public void Init()
        {
            CurrentValue = InitValue == 0 ? MaxValue : InitValue;
        }

        public void LogicUpdate() { }

        public override void Increase(float amount)
        {
            base.Increase(amount);
        }

        public override void Decrease(float amount)
        {
            base.Decrease(amount);
        } 
    }

    public enum ResourceType
    {
        Health,
        Mana,
    }
}