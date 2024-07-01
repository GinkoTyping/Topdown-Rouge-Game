using Ginko.CoreSystem;
using Ginko.Data;
using Ginko.EnemySystem;
using Ginko.StateMachineSystem;
using Shared.Utilities;
using UnityEngine;

public class E_IdleState : IdleState
{
    private Enemy enemy;
    private Timer timer;
    private bool isPatrolling;
    private Vector2 patrollingVector;
    private EnemyBasicDataSO enemyBasicData;

    public E_IdleState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
    {
        enemy = (Enemy)entity;
    }

    public override void Enter()
    {
        base.Enter();

        isPatrolling = false;
        enemyBasicData = (EnemyBasicDataSO)(enemy.EntityData);

        IniteTimer();
    }
    public override void Exit()
    {
        base.Exit();

        Entity.Anim.SetBool(AnimBoolName.Idle.ToString(), false);
        Entity.Anim.SetBool(AnimBoolName.Move.ToString(), false);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timer.Tick();
        if (Entity.Detections.IsHostileDetected)
        {
            StateMachine.ChangeState(Entity.HostileDetectedState);
        }
        else if (isPatrolling)
        {
            Entity.Movement.SetVelocity(Entity.EntityData.moveVelocity, patrollingVector);
        }
        else
        {
            Entity.Movement.SetVelocityZero();
        }
    }
    public override void RegisterEvents()
    {
        base.RegisterEvents();

        enemy.OnObstaclesCollision += OnObstaclesCollision;
    }
    public override void UnRegisterEvents()
    {
        base.UnRegisterEvents();

        enemy.OnObstaclesCollision += OnObstaclesCollision;
    }


    private void OnObstaclesCollision()
    {
        IniteTimer();
        isPatrolling = true;
        patrollingVector = -patrollingVector;
    }

    private void IniteTimer()
    {
        timer = new Timer(enemyBasicData.idleTimeDuration);
        timer.StartTimer();
        timer.OnTimerDone += SwitchIdleMode;
    }

    private void SwitchIdleMode()
    {
        isPatrolling = !isPatrolling;

        Entity.Anim.SetBool("Move", isPatrolling);
        Entity.Anim.SetBool("Idle", !isPatrolling);

        if (isPatrolling)
        {
            patrollingVector = GetRamdonDirection();
        }

        IniteTimer();
    }

    private Vector2 GetRamdonDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        return new Vector2(x, y);
    }
}
