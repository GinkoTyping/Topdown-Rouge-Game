using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newResourceBuffData", menuName = "Data/Buff/Resource")]
public class ResourceBuffDataSO : BaseBuffDataSO
{
    [SerializeField] public AttributeType resourceType;
    [SerializeField] public GameObject buffEffect;
    [SerializeField] public float perValue;
    [SerializeField] public float perTime;
    [SerializeField] public float totalTime;
}

public class ResourceChangeData
{
    [SerializeField] public float perValue;
    [SerializeField] public float perTime;
    [SerializeField] public float totalTime;
    public float passedTime;
    public float calculateTime;

    public ResourceChangeData(float value, float time, float totalTime)
    {
        perValue = value;
        perTime = time;
        this.totalTime = totalTime;

        passedTime = 0;
        calculateTime = 0;
    }
}