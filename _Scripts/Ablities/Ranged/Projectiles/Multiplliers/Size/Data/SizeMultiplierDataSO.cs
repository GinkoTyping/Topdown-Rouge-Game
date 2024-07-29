using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSizeMultiplierData", menuName = "Data/Projectile Multiplier/Size Multiplier Data")]
public class SizeMultiplierDataSO : ScriptableObject
{
    [SerializeField] public bool byRange;
    [SerializeField] public float[] range = new float[2];
    [SerializeField] public bool bySpecificRange;
    [SerializeField] public specificRange[] specificRanges;

    [Serializable]
    public class specificRange
    {
        public float value;
        public float possibility;
    }
}
