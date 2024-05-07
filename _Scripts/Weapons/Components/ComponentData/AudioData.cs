using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class AudioData : ComponentData<AttackAudio>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Audio);
        }
    }
}