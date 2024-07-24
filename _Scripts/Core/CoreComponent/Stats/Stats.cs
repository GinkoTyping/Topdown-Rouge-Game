using Ginko.PlayerSystem;
using System;
using System.Xml.Linq;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Stats : CoreComponent
    {
        [field: SerializeField] public ResourceStat Health {  get; private set; }
        [field: SerializeField] public ResourceStat Poise {  get; private set; }

        public override void OnEnable()
        {
            base.OnEnable();
            
            if (Core.GetComponentInParent<Player>() == null )
            {
                Health.Init();
                Poise.Init();
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Health.LogicUpdate();
            Poise.LogicUpdate();
        }
        public ResourceStat GetAttribute(ResourceType type)
        {
            ResourceStat output = null;
            if (type == ResourceType.Health)
            {
                output = Health;
            }

            return output;
        }
    }
}

