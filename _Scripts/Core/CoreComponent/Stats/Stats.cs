using Ginko.PlayerSystem;
using System;
using System.Xml.Linq;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Stats : CoreComponent
    {
        [SerializeField]
        protected HealthBar healthBar;
        [field: SerializeField] public ResourceStat Health {  get; private set; }
        [field: SerializeField] public ResourceStat Poise {  get; private set; }
        [SerializeField]
        public AttributeStat[] attributes;

        private FloatingText floatingTextComp;

        public override void OnEnable()
        {
            base.OnEnable();

            if (healthBar != null )
            {
                Health.OnCurrentValueChange += healthBar.ChangeHealthBar;
            }

            Health.Init();
            Poise.Init();

            if (healthBar != null)
            {
                healthBar.ChangeHealthBar(Health.CurrentValue, Health.MaxValue, 0);
            }

            SetHealingFloatingText();
        }

        private void OnDisable()
        {
            if (healthBar != null )
            {
                Health.OnCurrentValueChange -= healthBar.ChangeHealthBar;
            }

            if (floatingTextComp != null)
            {
                Health.OnCurrentValueChange -= floatingTextComp.FloatHealText;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Health.LogicUpdate();
            Poise.LogicUpdate();
        }

        private void SetHealingFloatingText()
        {
            if (floatingTextComp == null)
            {
                floatingTextComp = Core.GetCoreComponent<FloatingText>();
            }

            if (floatingTextComp != null)
            {
                Health.OnCurrentValueChange += floatingTextComp.FloatHealText;
            }
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
        public AttributeStat GetAttribute(AttributeType type)
        {
            AttributeStat output = null;
            foreach (AttributeStat attribute in attributes)
            {
                if (attribute.type == type)
                {
                    output = attribute;
                    break;
                }
            }

            return output;
        }
    }
}

