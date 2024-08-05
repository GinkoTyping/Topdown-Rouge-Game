using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "newResourceBuffData", menuName = "Data/Buff/Resource")]
public class StatusBuffDataSO : BaseBuffDataSO
{
    [Header("Type")]
    [SerializeField] public bool isAttribute;
    [SerializeField] public ResourceType resourceType;
    [SerializeField] public AttributeType attributeType;
    [SerializeField] public bool isUpdateOverTime;

    [Header("Resource")]
    [SerializeField] public float staticValue;
    [SerializeField] public float perValue;
    [SerializeField] public float perTime;
    [SerializeField] public float totalTime;

    public enum VFX_ActiveType
    {
        DuringActive,
        OnActivate,
    }
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