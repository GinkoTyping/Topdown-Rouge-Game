using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newResourceBuffData", menuName = "Data/Buff/Life Steal")]
public class LifeStealingBuffDataSO : BaseBuffDataSO
{
    [SerializeField] public LifeStealType type;
    [SerializeField] public float value;
}

public enum LifeStealType
{
    Amount,
    Percentage,
}
