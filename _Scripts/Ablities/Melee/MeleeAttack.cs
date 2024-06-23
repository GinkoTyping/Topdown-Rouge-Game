using Ginko.CoreSystem;
using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : BaseAbility
{
    [Header("Melee Attack")]
    [SerializeField] public AttackDetail[] attackDetails;

    private int atttackCounter;
    private FiniteStateMachine stateMachine;

    private Movement movement;
    private Detections detections;

    private void Start()
    {
        movement = GetComponentInParent<Core>().GetCoreComponent<Movement>();
        detections = GetComponentInParent<Core>().GetCoreComponent<Detections>();

        
        stateMachine = GetComponentInParent<Entity>().StateMachine;
    }

    public override void BeforeActivate()
    {
        base.BeforeActivate();

        animationEventHandler.OnFinish += AddAttackCounter;
        animationEventHandler.OnAttackAction += DetectDamegable;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        animationEventHandler.OnFinish -= AddAttackCounter;
        animationEventHandler.OnAttackAction -= DetectDamegable;
    }

    public override void Activate()
    {
        UpdateAnim(AnimBoolName.MeleeAttack);
    }

    private void DetectDamegable()
    {
        atttackCounter = animator.GetInteger("AttackCounter");
        AttackDetail currentAttack = attackDetails[atttackCounter];
        Collider2D[] colliders = Physics2D.OverlapBoxAll(currentAttack.GetCenterPoint(transform.position, movement.FacingDirection), currentAttack.hitBoxSize, 0, hostileLayer);

        foreach (var collider in colliders)
        {
            IDamageable damageble = collider.transform.parent.GetComponent<IDamageable>();
            if (damageble != null)
            {
                damageble.Damage(currentAttack.damageAmount);
            }
        }
    }

    private void AddAttackCounter()
    {
        if (detections.IsInMeleeAttackRange)
        {
            int counter = animator.GetInteger("AttackCounter") + 1;
            if (counter > attackDetails.Length - 1)
            {
                counter = 0;
                UpdateAnim(AnimBoolName.Idle);
            }
            else
            {
                stateMachine.CurrentState.IsAnimationFinished = false;
            }
            animator.SetInteger("AttackCounter", counter);
        }
        else
        {
            stateMachine.CurrentState.IsAnimationFinished = true;
        }
    }
    
    private void OnDrawGizmos()
    {
        foreach (var attackDetail in attackDetails)
        {
            if (attackDetail.isDebug)
            {
                Vector2 center = new Vector2(transform.position.x + attackDetail.hitBoxOffset.x, transform.position.y + attackDetail.hitBoxOffset.y);
                Gizmos.DrawWireCube(center, attackDetail.hitBoxSize);
            }
        }
    }
}
