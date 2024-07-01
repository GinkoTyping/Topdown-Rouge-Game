using Ginko.CoreSystem;
using Ginko.StateMachineSystem;

public class DeathState : State
{
    public BaseAbility[] deathrattles;
    public DeathState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
    {
        deathrattles = entity.Core.GetCoreComponent<Death>().GetComponentsInChildren<BaseAbility>();
    }

    public override void Enter()
    {
        base.Enter();
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

        InitiateDeathRattles();
        Exit();
        Entity.Death.Die();
    }
    
    private void InitiateDeathRattles()
    {
        if (deathrattles.Length > 0)
        {
            foreach (BaseAbility ability in deathrattles)
            {
                ability.Activate();
            }
        }
    }
}
