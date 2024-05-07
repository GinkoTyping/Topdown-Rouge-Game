using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class Audio : WeaponComponent<AudioData, AttackAudio>
    {
        protected override void Start()
        {
            base.Start();

            EventHandler.OnEnterAttackPhase += PlayAttackAudio;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventHandler.OnEnterAttackPhase -= PlayAttackAudio;
        }

        private void PlayAttackAudio(AttackPhases attackPhases)
        {
            SoundManager.Instance.PlaySound(currentAttackData.attackAudio);
        }
    }
}