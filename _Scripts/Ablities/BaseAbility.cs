using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] protected bool isDebug;
    [SerializeField] public AudioClip abilityAudio;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected LayerMask hostileLayer;

    protected Animator animator;
    protected AnimationEventHandler animationEventHandler;

    protected virtual void Awake()
    {
        animator = GetComponentInParent<Animator>();
        animationEventHandler = GetComponentInParent<AnimationEventHandler>();
    }

    /// <summary>
    /// 注册 Ability 相关的动画事件
    /// </summary>
    public virtual void BeforeActivate() { }

    /// <summary>
    /// 取消注册 Ability 相关的动画事件
    /// </summary>
    public virtual void Deactivate() { }

    public abstract void Activate();

    protected void UpdateAnim(AnimBoolName animBoolName)
    {
        
        animator.SetBool(AnimBoolName.Idle.ToString(),
            animBoolName == AnimBoolName.Idle);

        Debug.Log($"{animBoolName}, {animator.GetBool(AnimBoolName.Idle.ToString())}");

        animator.SetBool(AnimBoolName.Charge.ToString(),
            animBoolName == AnimBoolName.Charge);

        animator.SetBool(AnimBoolName.RangedAttack.ToString(),
            animBoolName == AnimBoolName.RangedAttack);

        animator.SetBool(AnimBoolName.MeleeAttack.ToString(),
            animBoolName == AnimBoolName.MeleeAttack);
    }
}
