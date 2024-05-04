using UnityEngine;

namespace Ginko.CoreSystem
{
    public class PerspecrtiveSprite : CoreComponent
    {
        [SerializeField]
        private float tintFadeSpeed;
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
            UpdateSprite();
        }
        private void OnEnable()
        {
            ChangeSpritesColor(Color.clear);
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

        private void UpdateSprite()
        {
            if (hasChangedColor)
            {
                Color currentColor = entitySpriteRenderer.material.GetColor("_Tint");
                if (currentColor.a > 0)
                {
                    currentColor.a = Mathf.Clamp01(currentColor.a - tintFadeSpeed * Time.deltaTime);
                    ChangeSpritesColor(currentColor);
                }
                else
                {
                    hasChangedColor = false;
                }
            }
        }

        public void TintSprite(Color color)
        {
            hasChangedColor = true;
            ChangeSpritesColor(color);
        }

        private void ChangeSpritesColor(Color color)
        {
            entitySpriteRenderer.material.SetColor("_Tint", color);
            baseWeaponSpriteRenderer?.material.SetColor("_Tint", color);
            weaponSpriteRenderer?.material.SetColor("_Tint", color);
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