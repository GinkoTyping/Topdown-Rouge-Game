using Ginko.Weapons.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class AttackSprites : AttackData
{
    [field: SerializeField] public PhaseSprites[] PhaseSprites {  get; private set; }
}

[Serializable]
public struct PhaseSprites
{
    [field: SerializeField] public AttackPhases Phase { get; private set; }
    [field: SerializeField] public Sprite[] Sprites { get; private set; }
}