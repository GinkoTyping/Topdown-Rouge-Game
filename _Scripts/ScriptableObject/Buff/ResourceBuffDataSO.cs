using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "newResourceBuffData", menuName = "Data/Buff/Resource")]
public class ResourceBuffDataSO : BaseBuffDataSO
{
    [SerializeField] public ResourceType resourceType;

    [Header("VFX")]
    [SerializeField] public GameObject buffEffect;
    [SerializeField] public VFX_ActiveType vfx_actvieType;
    [SerializeField] public float vfx_activeTime;
    [SerializeField] public Vector3 vfx_offset;
    [SerializeField] public Vector3 vfx_scale = Vector3.one;

    [Header("Resource")]
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