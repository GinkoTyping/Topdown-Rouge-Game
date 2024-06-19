using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedAttack
{
    Status statusIndex { get; }
}
public enum Status
{
    Idle,
    Charge,
    Attack,
}