using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class SpritesHandler : CoreComponent
    {
        private SpriteRenderer entitySpriteRenderer;
        private SpriteRenderer baseWeaponSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;
        public Detections Detections
        {
            get => detections ??= Core.GetCoreComponent<Detections>();
        }
        private Detections detections;

        private bool HasAddedSortingOrder;

        protected override void Awake()
        {
            base.Awake();

            HasAddedSortingOrder = false;
            entitySpriteRenderer = transform.parent.parent.GetComponent<SpriteRenderer>();
            GameObject primaryWeapon = transform.parent.parent.Find("PrimaryWeapon")?.gameObject;
            if (primaryWeapon != null)
            {
                baseWeaponSpriteRenderer = primaryWeapon.transform.Find("Base").GetComponent<SpriteRenderer>();
                weaponSpriteRenderer = primaryWeapon.transform.Find("WeaponSprite").GetComponent<SpriteRenderer>();
            }
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Detections.IsCollidingUpper && !HasAddedSortingOrder)
            {
                HasAddedSortingOrder = true;
                entitySpriteRenderer.sortingOrder++;
                if (baseWeaponSpriteRenderer != null && weaponSpriteRenderer != null)
                {
                    baseWeaponSpriteRenderer.sortingOrder++;
                    weaponSpriteRenderer.sortingOrder++;
                }
            }
            else if (!Detections.IsCollidingUpper && HasAddedSortingOrder)
            {
                HasAddedSortingOrder = false;
                entitySpriteRenderer.sortingOrder--;
                if (baseWeaponSpriteRenderer != null && weaponSpriteRenderer != null)
                {
                    baseWeaponSpriteRenderer.sortingOrder--;
                    weaponSpriteRenderer.sortingOrder--;
                }
            }
        }
    }
}