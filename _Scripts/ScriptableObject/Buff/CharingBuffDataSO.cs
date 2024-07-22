using Ginko.CoreSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackIntervalChargeData", menuName = "Data/Multiplier/Attack Interval Charge Data")]
public class CharingBuffDataSO : BaseBuffDataSO
{
    [SerializeField] public ChargingBaseType chargingBaseType;
    [SerializeField] public AttributeType chargingTargetAttribute;

    [SerializeField] public float maxStack;
    [SerializeField] public float timePerStack;
    [SerializeField] public float modifier;
}

public enum ChargingBaseType
{
    // ��������
    ContinousAttack,

    // ��ֹ
    NotMoving,

    // �����ƶ�
    Moving,
}
