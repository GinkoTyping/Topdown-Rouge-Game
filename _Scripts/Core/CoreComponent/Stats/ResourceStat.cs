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
    }

    public enum ResourceType
    {
        Health,
        Mana,
    }
}