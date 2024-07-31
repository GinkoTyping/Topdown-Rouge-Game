using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Ginko.StateMachineSystem;
using System;
using UnityEngine;

public class MeleeAttack : BaseAbility
{
    [Header("Melee Attack")]
    [SerializeField] public AttackDetail[] attackDetails;

    private int atttackCounter;

    private Movement movement;
    private Detections detections;

    protected override void Start()
    {
        movement = GetComponentInParent<Core>().GetCoreComponent<Movement>();
        detections = GetComponentInParent<Core>().GetCoreComponent<Detections>();

        
        stateMachine = GetComponentInParent<Entity>().StateMachine;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        animationEventHandler.OnFinish += AddAttackCounter;
        animationEventHandler.OnAttackAction += DetectDamegable;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        animationEventHandler.OnFinish -= AddAttackCounter;
        animationEventHandler.OnAttackAction -= DetectDamegable;
    }

    public override void Activate()
    {
        movement.FaceToItem(Player.Instance.transform);

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
                stateMachine.CurrentState.IsAnimationFinished = true;
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

[Serializable]
public class AttackDetail
{
    [SerializeField]
    public bool isDebug;

    [SerializeField]
    public float damageAmount;
    [SerializeField]
    public float moveVelocity;
    [SerializeField]
    public Vector2 hitBoxSize;
    [SerializeField]
    public Vector2 hitBoxOffset;

    public Vector2 GetCenterPoint(Vector3 origin, int facingDirection)
    {
        return new Vector2(origin.x + hitBoxOffset.x * facingDirection, origin.y + hitBoxOffset.y);
    }
}