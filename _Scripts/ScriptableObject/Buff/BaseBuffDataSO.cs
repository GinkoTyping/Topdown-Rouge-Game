using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuffDataSO : ScriptableObject
{
    [SerializeField] public Sprite iconSprite;
    [SerializeField] public bool hasTimer;
    [SerializeField] public bool stackable;
} 
