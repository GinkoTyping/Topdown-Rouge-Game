using System;
using System.Xml.Linq;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Stats : CoreComponent
    {
        [field: SerializeField] public ResouceStat Health {  get; private set; }
        [field: SerializeField] public ResouceStat Poise {  get; private set; }

        private void OnEnable()
        {
            Health.Init();
            Poise.Init();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Health.LogicUpdate();
            Poise.LogicUpdate();
        }
        public ResouceStat GetAttribute(ResourceType type)
        {
            ResouceStat output = null;
            if (type == ResourceType.Health)
            {
                output = Health;
            }

            return output;
        }
    }
}

