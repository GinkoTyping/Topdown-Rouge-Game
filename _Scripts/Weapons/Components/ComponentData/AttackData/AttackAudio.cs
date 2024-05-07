using UnityEngine;
using System;

namespace Ginko.Weapons.Components
{
    [Serializable]
    public class AttackAudio : AttackData
    {
        [field: SerializeField] public AudioClip attackAudio { get; private set; }
    }
}