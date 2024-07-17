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
    // ��������
    ContinousAttack,

    // ��ֹ
    NotMoving,

    // �����ƶ�
    Moving,
}
