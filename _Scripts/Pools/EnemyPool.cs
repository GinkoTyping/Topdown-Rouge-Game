using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : PoolManager
{
    public override void SetPoolReference(GameObject obj)
    {
        obj.GetComponent<Entity>().SetPool(Pool);
    }
}
