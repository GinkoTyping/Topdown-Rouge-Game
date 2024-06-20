using Ginko.CoreSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    private IDeathrattle deathrattleAbility;
    public DeathState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
    {
        deathrattleAbility = entity.GetComponentInChildren<IDeathrattle>();
    }

    public override void Enter()
    {
        base.Enter();

        deathrattleAbility.Deathrattle();
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
