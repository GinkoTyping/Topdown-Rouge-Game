using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAblity
{
    public event Action OnCharge;
    public event Action OnAttack;

    void Activate();
}
