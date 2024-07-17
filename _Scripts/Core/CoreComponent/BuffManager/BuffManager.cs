using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : CoreComponent
{
    public List<Buff> buffList;

    public PlayerStats stats { get; private set; }
    public NormalAttack normalAttack { get; private set; }

    public float continousAttackTime;
    public float continousStayingTime;
    public float continousMovingTime;

    private Player player;

    private void Start()
    {
        player = Core.GetComponentInParent<Player>();
        stats = Core.GetCoreComponent<PlayerStats>();
        normalAttack = Core.GetCoreComponent<NormalAttack>();
    }

    public void Add(Buff buff)
    {
        buffList.Add(buff);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateContinousAttackTime();
        UpdateBuffs();
    }

    private void UpdateBuffs()
    {
        foreach (Buff buff in buffList)
        {
            buff.LogicUpdate();
        }
    }

    private void UpdateContinousAttackTime()
    {
        if (player.IsAttackInput)
        {
            continousAttackTime += Time.deltaTime;
        }
        else if (continousAttackTime != 0)
        {
            continousAttackTime = 0;
        }
    }
}
