using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class PlayerStats : Stats
    {
        public override void OnEnable()
        {
            base.OnEnable();

            foreach (AttributeStat attribute in attributes)
            {
                attribute.Init();
            }
        }

        private void OnDisable()
        {
            
        }
    }
}