using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileMultiplier : MonoBehaviour
{
    public abstract Projectile Apply(Projectile projectile);
}
