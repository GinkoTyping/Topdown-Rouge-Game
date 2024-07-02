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

        public override void OnEnable()
        {
            base.OnEnable();

            foreach (AttributeStat attribute in attributes)
            {
                attribute.Init();
            }
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

        private float GetAttackSpeed()
        {
            return 0;
        }
    }
}