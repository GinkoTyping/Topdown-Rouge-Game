using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class DamageData : ComponentData<AttackDamage>
    {

        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Damage);
        }
    }
}