using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class InputHoldData : ComponentData
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(InputHold);
        }
    }
}