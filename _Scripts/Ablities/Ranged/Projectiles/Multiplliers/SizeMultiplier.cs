using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SizeMultiplier : ProjectileMultiplier
{
    [SerializeField] bool byRange;
    [SerializeField] float[] range = new float[2];
    [SerializeField] bool bySpecificRange;
    [SerializeField] specificRange[] specificRanges;

    private PossibilityHelper possibilityHelper;

    [Serializable]
    private class specificRange
    {
        public float value;
        public float possibility;
    }

    private void Awake()
    {
        possibilityHelper = new PossibilityHelper();
    }

    public override Projectile Apply(Projectile projectile)
    {
        float sizeMultiplier = 1;

        if (byRange)
        {
            sizeMultiplier = UnityEngine.Random.Range(range[0], range[1]);
        }
        else if (bySpecificRange)
        {
            int index = possibilityHelper.Get(specificRanges.Select(item => item.possibility).ToArray());
            sizeMultiplier = index == -1 ? 1 : specificRanges[index].value;
        }

        projectile.gameObject.transform.localScale = new Vector3 (sizeMultiplier, sizeMultiplier, sizeMultiplier);
        return projectile;
    }
}
