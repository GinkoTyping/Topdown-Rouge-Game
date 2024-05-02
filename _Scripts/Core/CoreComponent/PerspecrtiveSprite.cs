using Ginko.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class PerspecrtiveSprite : CoreComponent
    {
        public Detections Detections
        {
            get => detections ??= Core.GetCoreComponent<Detections>();
        }
        private Detections detections;

        private SpriteRenderer entitySpriteRenderer;
        private SpriteRenderer baseWeaponSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;

        private GameObject primaryWeapon;
        private bool hasAddedSortingOrder;

        private bool hasChangedColor;
        private float resetColorTime;

        private Collider2D[] lastHidingObjects;

        protected override void Awake()
        {
            base.Awake();

            hasAddedSortingOrder = false;
            entitySpriteRenderer = transform.parent.parent.GetComponent<SpriteRenderer>();
            primaryWeapon = transform.parent.parent.Find("PrimaryWeapon")?.gameObject;

            if (primaryWeapon != null)
            {
                baseWeaponSpriteRenderer = primaryWeapon.transform.Find("Base").GetComponent<SpriteRenderer>();
                weaponSpriteRenderer = primaryWeapon.transform.Find("WeaponSprite").GetComponent<SpriteRenderer>();
            }
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            SetSpriteRenderOrder();
            SetSpriteColor();
        }
        private void OnEnable()
        {
            Detections.OnHidingBehind += HidingBehindSprite;
        }

        private void OnDisable()
        {
            Detections.OnHidingBehind -= HidingBehindSprite;
        }

        private void SetSpriteRenderOrder()
        {
            if (Detections.IsCollidingUpper && !hasAddedSortingOrder)
            {
                hasAddedSortingOrder = true;
                entitySpriteRenderer.sortingOrder++;
                if (primaryWeapon != null)
                {
                    baseWeaponSpriteRenderer.sortingOrder++;
                    weaponSpriteRenderer.sortingOrder++;
                }
            }
            else if (!Detections.IsCollidingUpper && hasAddedSortingOrder)
            {
                hasAddedSortingOrder = false;
                entitySpriteRenderer.sortingOrder--;
                if (primaryWeapon != null)
                {
                    baseWeaponSpriteRenderer.sortingOrder--;
                    weaponSpriteRenderer.sortingOrder--;
                }
            }
        }

        private void SetSpriteColor()
        {
            if (hasChangedColor && Time.time >= resetColorTime)
            {
                entitySpriteRenderer.color = Color.white;
                if (primaryWeapon != null)
                {
                    baseWeaponSpriteRenderer.color = Color.white;
                    weaponSpriteRenderer.color = Color.white;
                }
            }
        }
        public void ChangeSpriteColor(Color color, float time)
        {
            hasChangedColor = true;
            resetColorTime = Time.time + time;

            entitySpriteRenderer.color = color;
            if (primaryWeapon != null)
            {
                baseWeaponSpriteRenderer.color = color;
                weaponSpriteRenderer.color = color;
            }
        }
        public void HidingBehindSprite(Collider2D[] colliders)
        {
            if (colliders.Length > 0)
            {
                lastHidingObjects = colliders;
                foreach (Collider2D collider in lastHidingObjects)
                {
                    collider.GetComponent<SpriteHandler>().HideSprite(true);
                }
            } else if (lastHidingObjects?.Length > 0)
            {
                foreach (Collider2D collider in lastHidingObjects)
                {
                    collider.GetComponent<SpriteHandler>().HideSprite(false);
                }
                lastHidingObjects = null;
            }
        }
    }
}