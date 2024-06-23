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
    public abstract void Activate();
}
