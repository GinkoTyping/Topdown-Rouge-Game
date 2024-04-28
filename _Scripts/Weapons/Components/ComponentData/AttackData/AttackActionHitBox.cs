using Ginko.Weapons.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    [Serializable]
    public class AttackActionHitBox : AttackData
    {
        public bool debug;
        [field: SerializeField] public Rect HitBox { get; private set; }
    }

}
