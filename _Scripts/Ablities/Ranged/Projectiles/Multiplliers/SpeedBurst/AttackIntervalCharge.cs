using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using UnityEngine;

public class AttackIntervalCharge : ProjectileMultiplier
{
    [SerializeField] public CharingBuffData data;

    private AbilityManager abilityManager;
    private NormalAttack normalAttack;
    private Player player;

    private float currenrStack = 0f;
    private void Start()
    {
        abilityManager = GetComponent<AbilityManager>();
        normalAttack = GetComponent<NormalAttack>();
        player = transform.parent.GetComponentInParent<Player>();
    }

    public override Projectile Apply(Projectile projectile)
    {
        float stack = Mathf.Floor(normalAttack.continousAttackTime / data.timePerStack);
        if (stack <= data.maxStack && stack != currenrStack)
        {
            if (currenrStack != 0 && stack == 0f)
            {
                ResetBaseAttackInterval();
            } else
            {
                abilityManager.ModifyCooldown(abilityManager.totalCooldownTime * data.modifier);
            }

            currenrStack = stack;
        }
        
        return projectile;
    }

    private void ResetBaseAttackInterval()
    {
        AttributeStat attackInterval = player.Core.GetCoreComponent<PlayerStats>().GetAttribute(AttributeType.AttackInterval);
        abilityManager.ModifyCooldown(attackInterval.CurrentValue);
    }
}
