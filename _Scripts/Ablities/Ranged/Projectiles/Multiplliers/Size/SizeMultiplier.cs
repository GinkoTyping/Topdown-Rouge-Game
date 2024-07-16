using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SizeMultiplier : ProjectileMultiplier
{
    [SerializeField] private SizeMultiplierDataSO data;
    private PossibilityHelper possibilityHelper;

    private void Awake()
    {
        possibilityHelper = new PossibilityHelper();
    }

    public override Projectile Apply(Projectile projectile)
    {
        float sizeMultiplier = 1;

        if (data.byRange)
        {
            sizeMultiplier = UnityEngine.Random.Range(data.range[0], data.range[1]);
        }
        else if (data.bySpecificRange)
        {
            int index = possibilityHelper.Get(data.specificRanges.Select(item => item.possibility).ToArray());
            sizeMultiplier = index == -1 ? 1 : data.specificRanges[index].value;
        }

        projectile.gameObject.transform.localScale = new Vector3 (sizeMultiplier, sizeMultiplier, sizeMultiplier);
        return projectile;
    }
}
