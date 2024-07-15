using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class SpriteEffect : CoreComponent
    {
        [Header("Tint")]
        [SerializeField] private float tintFadeSpeed;
        [SerializeField] public Color warningColor;

        [Header("Player Only")]
        [SerializeField] private string backgroundLayerName;
        [SerializeField] private string selfLayerName;
        [SerializeField] private int beforeSortingOrder;
        [SerializeField] private GameObject playerLightGO;

        [Header("Obstables Only")]
        [SerializeField] private HidingBehindBehavior[] hidingBehindBehaviors;
        [SerializeField] private SpriteRenderer[] sprites;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color hidingColor;
        private enum HidingBehindBehavior
        {
            None,
            Transparent,
            DisableLight,
        }

        private Detections Detections;

        private SpriteRenderer entitySpriteRenderer;

        private bool hasAddedSortingOrder;

        private bool hasChangedColor;
        private Color tintColor;
        private int tintCurrentCount;

        private Collider2D[] lastHidingObjects;

        protected override void Awake()
        {
            base.Awake();

            hasAddedSortingOrder = false;
            entitySpriteRenderer = transform.parent.parent.GetComponent<SpriteRenderer>();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            SetSpriteRenderOrder();
            UpdateSprite();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            ChangeSpritesColor(Color.clear);

            Detections = Core.GetCoreComponent<Detections>();
            if (Detections != null)
            {
                Detections.OnHidingBehind += HidingBehindSprite;
            }
        }

        private void OnDisable()
        {
            if (Detections != null)
            {
                Detections.OnHidingBehind -= HidingBehindSprite;
            }
        }

        private void SetSpriteRenderOrder()
        {
            if (Detections.IsCollidingUpper 
                && !hasAddedSortingOrder)
            {
                hasAddedSortingOrder = true;
                entitySpriteRenderer.sortingLayerName = backgroundLayerName;
                entitySpriteRenderer.sortingOrder = beforeSortingOrder;
            }
            else if (!Detections.IsCollidingUpper 
                && hasAddedSortingOrder)
            {
                hasAddedSortingOrder = false;
                entitySpriteRenderer.sortingLayerName = selfLayerName;
                entitySpriteRenderer.sortingOrder = 0;
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
                    tintCurrentCount--;
                    if (tintCurrentCount == 0)
                    {
                        hasChangedColor = false;
                    } else
                    {
                        ChangeSpritesColor(tintColor);
                    }
                }
            }
        }

        public void TintSprite(Color color, int repeatCount = 1)
        {
            tintColor = color;
            hasChangedColor = true;
            tintCurrentCount = repeatCount;
            ChangeSpritesColor(color);
        }

        private void ChangeSpritesColor(Color color)
        {
            if (entitySpriteRenderer != null)
            {
                entitySpriteRenderer.material.SetColor("_Tint", color);
            }
        }

        public void HidingBehindSprite(Collider2D[] colliders)
        {
            if (colliders.Length > 0)
            {
                lastHidingObjects = colliders;
                SwitchColliderVisible(colliders, true);
            }
            else if (lastHidingObjects?.Length > 0)
            {
                SwitchColliderVisible(lastHidingObjects, false);
                lastHidingObjects = null;
            }
        }

        private void SwitchColliderVisible(Collider2D[] colliders ,bool isBehind)
        {
            foreach (Collider2D collider in colliders)
            {
                SpriteEffect spriteEffect = collider.transform.parent.GetComponent<SpriteEffect>();

                if (spriteEffect.hidingBehindBehaviors.Contains(HidingBehindBehavior.Transparent))
                {
                    spriteEffect.SwitchSpriteTransparence(isBehind);
                }

                if (spriteEffect.hidingBehindBehaviors.Contains(HidingBehindBehavior.DisableLight))
                {
                    playerLightGO.SetActive(!isBehind);
                }
            }
        }

        private void SwitchSpriteTransparence(bool isTransparent)
        {
            foreach (SpriteRenderer spriteRenderer in sprites)
            {
                spriteRenderer.color = isTransparent ? hidingColor : defaultColor;
            }
        }
    }
}