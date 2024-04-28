using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{

    [Serializable]
    public class AttackKnockBack : AttackData
    {
        [field: SerializeField] public Vector2 Angel {  get; private set; }
        [field: SerializeField] public float Strength {  get; private set; }
    }
}

