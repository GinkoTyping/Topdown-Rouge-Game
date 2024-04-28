using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
    {
        private SpriteRenderer baseSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;
        private int currentWeaponSpriteIndex;

        private Sprite[] currentPhaseSprites;
        private void HandleEnterAttackPhase(AttackPhases phase)
        {
            currentWeaponSpriteIndex = 0;
            currentPhaseSprites = currentAttackData.PhaseSprites.FirstOrDefault(data => data.Phase == phase).Sprites;
        }
        private void HandleBaseSpriteChanges(SpriteRenderer spriteRenderer)
        {
            if (isAttackActive)
            {
                if (currentWeaponSpriteIndex >= currentPhaseSprites.Length)
                {
                    Debug.LogWarning($"{weapon.name} weapon sprites length dismatch");
                }
                else
                {
                    weaponSpriteRenderer.sprite = currentPhaseSprites[currentWeaponSpriteIndex];
                    currentWeaponSpriteIndex++;
                }
            }
            else
            {
                weaponSpriteRenderer.sprite = null;
            }
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentWeaponSpriteIndex = 0;
        }

        protected override void Start()
        {
            base.Start();

            baseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();

            baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChanges);
            EventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChanges);
            EventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
        }
    }
}


