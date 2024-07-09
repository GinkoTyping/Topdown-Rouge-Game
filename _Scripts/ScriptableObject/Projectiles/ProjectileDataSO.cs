using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newProjectileData", menuName = "Data/Projectile/BaseProjectile")]
public class ProjectileDataSO : ScriptableObject
{
    [Header("- Sprites")]
    public Sprite sprite;
    public Material glowMaterial;

    [Header("- Rigidbody")]
    public float velocity;
    public float duaration;

    [Header("- Damage")]
    public float damageAmount;
    public Vector2 collisionOffset;
    public Vector2 collisionSize;

    [Header("- Possibility")]
    [Range(0,1)]
    public float possibility;
}
