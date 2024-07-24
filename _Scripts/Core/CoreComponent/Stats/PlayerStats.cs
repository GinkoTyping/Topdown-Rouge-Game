using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class PlayerStats : Stats
    {
        [SerializeField]
        public AttributeStat[] attributes;
        [SerializeField]
        private HealthBar playerHealthBar;

        public override void OnEnable()
        {
            base.OnEnable();

            Health.OnCurrentValueChange += playerHealthBar.ChangeHealthBar;

            Health.Init();
            Poise.Init();

            foreach (AttributeStat attribute in attributes)
            {
                attribute.Init();
            }
        }

        private void OnDisable()
        {
            Health.OnCurrentValueChange -= playerHealthBar.ChangeHealthBar;
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