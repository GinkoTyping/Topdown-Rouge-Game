using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class MovementData : ComponentData<AttackMovement>
    {

        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Movement);
        }
    }
}
