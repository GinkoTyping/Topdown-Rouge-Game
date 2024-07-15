using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeMultiplier : ProjectileMultiplier
{
    [SerializeField] float[] range = new float[2];

    public override Projectile Apply(Projectile projectile)
    {
        float sizeMultiplier = Random.Range(range[0], range[1]);
        projectile.gameObject.transform.localScale = new Vector3 (sizeMultiplier, sizeMultiplier, sizeMultiplier);
        return projectile;
    }
}
