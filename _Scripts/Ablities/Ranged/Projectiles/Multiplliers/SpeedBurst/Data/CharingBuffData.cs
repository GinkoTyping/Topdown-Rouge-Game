using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackIntervalChargeData", menuName = "Data/Multiplier/Attack Interval Charge Data")]
public class CharingBuffData : ScriptableObject
{
    [SerializeField] public ChargingType chargingType;

    [SerializeField] public float maxStack;
    [SerializeField] public float timePerStack;
    [SerializeField] public float modifier;
}
public enum ChargingType
{
    // 持续攻击
    ContinousAttack,

    // 静止
    NotMoving,

    // 保持移动
    Moving,
}
