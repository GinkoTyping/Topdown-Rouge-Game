using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    protected BuffManager buffManager;

    protected float currenrStack = 0f;
    protected virtual void Start()
    {
        buffManager = GetComponentInParent<BuffManager>();
        buffManager.Add(this);
    }

    public abstract void LogicUpdate();
}
