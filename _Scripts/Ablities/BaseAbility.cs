using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    [SerializeField] protected bool isDebug;
    [Header("Base")]
    [SerializeField] protected bool playAudioByAnim = true;
    [SerializeField] protected bool playAudioOnActivate;
    [SerializeField] public AudioClip abilityAudio;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected LayerMask hostileLayer;

    protected Entity entity;
    protected FiniteStateMachine stateMachine;
    protected Animator animator;
    protected AnimationEventHandler animationEventHandler;

    protected bool isAbleToActivate;

    protected virtual void Awake()
    {
        animator = GetComponentInParent<Animator>();
        animationEventHandler = GetComponentInParent<AnimationEventHandler>();
        entity = GetComponentInParent<Entity>();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnEnable()
    {
        SwitchAbleToActivate(true);
        if (playAudioByAnim)
        {
            animationEventHandler.OnAttackAction += PlayAbilitySound;
        }
    }

    protected virtual void OnDisable()
    {
        if (playAudioByAnim)
        {
            animationEventHandler.OnAttackAction -= PlayAbilitySound;
        }
    }

    protected virtual void Start()
    {
        stateMachine = GetComponentInParent<Entity>().StateMachine;
    }

    public void SwitchAbleToActivate(bool isAble)
    {
        isAbleToActivate = isAble;
    }

    public abstract void Activate();

    protected void UpdateAnim(AnimBoolName animBoolName)
    {
        animator.SetBool(AnimBoolName.Idle.ToString(),
            animBoolName == AnimBoolName.Idle);

        animator.SetBool(AnimBoolName.Charge.ToString(),
            animBoolName == AnimBoolName.Charge);

        animator.SetBool(AnimBoolName.RangedAttack.ToString(),
            animBoolName == AnimBoolName.RangedAttack);

        animator.SetBool(AnimBoolName.MeleeAttack.ToString(),
            animBoolName == AnimBoolName.MeleeAttack);
    }
    
    protected void PlayAbilitySound()
    {
        if (abilityAudio != null)
        {
            SoundManager.Instance.PlaySound(abilityAudio);
        }
    }
}
