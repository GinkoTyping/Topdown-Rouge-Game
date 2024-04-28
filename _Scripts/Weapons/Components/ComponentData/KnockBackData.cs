using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class KnockBackData : ComponentData<AttackKnockBack>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(KnockBack);
        }
    }
}