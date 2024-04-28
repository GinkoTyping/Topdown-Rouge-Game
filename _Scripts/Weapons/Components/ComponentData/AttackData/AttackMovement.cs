using System;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    [Serializable]
    public class AttackMovement: AttackData
    {
        [field: SerializeField] public Vector2 Direction { get; private set; }
        [field: SerializeField] public float Velocity { get; private set; }
    }
}
