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

            EventHandler.OnAttackAction += PlayAttackAudio;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventHandler.OnAttackAction -= PlayAttackAudio;
        }

        private void PlayAttackAudio()
        {
            SoundManager.Instance.PlaySound(currentAttackData.attackAudio);
        }
    }
}