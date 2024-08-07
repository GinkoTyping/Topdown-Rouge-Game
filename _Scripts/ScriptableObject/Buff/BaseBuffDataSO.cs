using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatusBuffDataSO;

public abstract class BaseBuffDataSO : ScriptableObject
{
    [SerializeField] public Sprite iconSprite;
    [SerializeField][TextArea] public string desc;
    [SerializeField] public bool hasTimer;
    [SerializeField] public bool stackable;

    [Header("VFX")]
    [SerializeField] public GameObject buff_vfx;
    [SerializeField] public VFX_ActiveType vfx_actvieType;
    [SerializeField] public float vfx_activeTime;
    [SerializeField] public Vector3 vfx_offset;
    [SerializeField] public Vector3 vfx_scale = Vector3.one;

    [Header("Audio")]
    [SerializeField] public bool autoPlayAudio;
    [SerializeField] public AudioClip audio;
} 
