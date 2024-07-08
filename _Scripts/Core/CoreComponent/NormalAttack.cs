using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.PlayerSystem;

namespace Ginko.CoreSystem
{
    public class NormalAttack : CoreComponent
    {
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material glowMaterial;

        private Material currentMaterial;

        private AbilityManager abilityManager;
        private Player player;
        private SpriteRenderer spriteRender;
        private AttributeStat attackInterval;

        protected override void Awake()
        {
            base.Awake();

            abilityManager = GetComponent<AbilityManager>();
            currentMaterial = normalMaterial;
        }

        private void Start()
        {
            player = Core.GetComponentInParent<Player>();
            spriteRender = player.GetComponent<SpriteRenderer>();
            attackInterval = player.Core.GetCoreComponent<PlayerStats>().GetAttribute(AttributeType.AttackInterval);

            abilityManager.SetCooldown(attackInterval.CurrentValue);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            InstaniateAttack();
            SetAttackSpriteColor();
        }

        private void InstaniateAttack()
        {
            if (player.IsAttackInput)
            {
                abilityManager.CheckIfAttack();
            }
        }

        private void SetAttackSpriteColor()
        {
            if (player.IsAttackInput && currentMaterial == normalMaterial)
            {
                currentMaterial = glowMaterial;
                spriteRender.material = currentMaterial;
            }
            else if (!player.IsAttackInput && currentMaterial == glowMaterial)
            {
                currentMaterial = normalMaterial;
                spriteRender.material = currentMaterial;
            }
        }
    }
}