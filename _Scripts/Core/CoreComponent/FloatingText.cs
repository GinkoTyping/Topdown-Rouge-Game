using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class FloatingText : CoreComponent
    {
        [SerializeField] private Vector2 defaultOffset = new Vector2 (0.6f, 0);

        private PoolManager poolManager;

        protected override void Awake()
        {
            base.Awake();

            poolManager = GetComponent<PoolManager>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public void FloatDamageText(DamageDetail damageDetail)
        {
            string content = damageDetail.isCritical
                ? $"{Mathf.Abs(damageDetail.amount)}!"
                : Mathf.Abs(damageDetail.amount).ToString();

            FontStyles fontStyles = FontStyles.Bold;
            float fontSize = 0f;

            if (damageDetail.isCritical)
            {
                fontStyles = FontStyles.Bold | FontStyles.Italic;
                fontSize = 0.8f;
            }


            Color color = Color.red;
            switch (damageDetail.damageEffect)
            {
                case DamageEffect.Normal:
                    break;
                case DamageEffect.Fire:
                    break;
                case DamageEffect.Ice:
                    color = Color.blue;
                    break;
                case DamageEffect.Toxic:
                    color = Color.green;
                    break;
                default:
                    break;
            }

            GameObject textGO = poolManager.Pool.Get();
            textGO.GetComponent<AnimatedText>().Init(
                poolManager,
                defaultOffset,
                content,
                color,
                fontStyles,
                fontSize);
        }

        public void FloatHealText(float currentValue, float maxValue, float valueBeforeChange)
        {
            if (valueBeforeChange < currentValue && poolManager != null)
            {
                GameObject textGO = poolManager.Pool.Get();
                textGO.GetComponent<AnimatedText>().Init(
                    poolManager,
                    defaultOffset,
                    (currentValue - valueBeforeChange).ToString(),
                    Color.green,
                    flipped: true);
            }
        }
    }
}