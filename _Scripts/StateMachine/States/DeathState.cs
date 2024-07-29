using Ginko.CoreSystem;
using Ginko.StateMachineSystem;

public class DeathState : State
{
    public BaseAbility[] deathrattles;
    public int finishedDeathRattleCount;

    public DeathState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
    {
        deathrattles = entity.Core.GetCoreComponent<Death>().GetComponentsInChildren<BaseAbility>();
    }

    public override void Enter()
    {
        base.Enter();

        finishedDeathRattleCount = 0;
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckDeathrRattlesDone();
    }

    private void OnDeathAnimFinish()
    {
        animEventHandler.OnFinish -= OnDeathAnimFinish;

        if (deathrattles.Length > 0 )
        {
            InitiateDeathRattles();
        } else
        {
            DestroyObject();
        }
    }
    
    private void InitiateDeathRattles()
    {
        if (deathrattles.Length > 0)
        {
            foreach (BaseAbility ability in deathrattles)
            {
                ability.OnAbilityFinished += HandleDeathRattleDone;
                ability.Activate();
            }
        }
    }

    private void HandleDeathRattleDone(BaseAbility ablity)
    {
        ablity.OnAbilityFinished -= HandleDeathRattleDone;
        finishedDeathRattleCount++;
    }

    private void CheckDeathrRattlesDone()
    {
        if (deathrattles.Length > 0 && finishedDeathRattleCount == deathrattles.Length)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Exit();
        Entity.Death.Die();
    }
}
