using Ginko.CoreSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    private BaseAbility[] deathrattles;
    public DeathState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
    {
        deathrattles = entity.Core.GetCoreComponent<Death>().GetComponentsInChildren<BaseAbility>();
    }

    public override void Enter()
    {
        base.Enter();

        if (deathrattles.Length > 0)
        {
            foreach (BaseAbility ability in deathrattles)
            {
                ability.Activate();
            }
        }
    }

    public override void RegisterEvents()
    {
        base.RegisterEvents();

        animEventHandler.OnFinish += OnDeathAnimFinish;
    }

    protected override void SetAnimBoolName()
    {
        AnimBoolName = AnimBoolName.Death;
    }

    private void OnDeathAnimFinish()
    {
        animEventHandler.OnFinish -= OnDeathAnimFinish;

        Exit();
        Entity.Death.Die();
    }
}
