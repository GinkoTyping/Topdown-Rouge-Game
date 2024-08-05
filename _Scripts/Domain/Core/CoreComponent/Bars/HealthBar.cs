using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ginko.CoreSystem
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private float lerpDuration;
        [SerializeField] private float originalWidth;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private TextMeshProUGUI healthTextMesh;
        [SerializeField] private HealthToColor[] healthToColors;
        private float lerpTotalTime;
        private bool isLerp;
        private bool isHover;

        private RectTransform HealthRect;
        private RectTransform HealthDecreasedRect;
        
        private float currentWidth;
        private float targerWidth = -1f;

        private float currentHealth;
        private float maxHealth;
        private Color currentHealthBarColor;

        [Serializable]
        public class HealthToColor
        {
            public Color color;
            public float[] range = new float[2];
        }

        protected void Awake()
        {
            HealthRect = transform.Find("Health").GetComponent<RectTransform>();
            HealthDecreasedRect = transform.Find("Health_Decreased")?.GetComponent<RectTransform>();

            if (originalWidth == 0f)
            {
                originalWidth = HealthRect.sizeDelta.x;
            }
        }

        private void Start()
        {
        }

        private void Update()
        {
            SwitchHealthTextVisible();

            if (isLerp)
            {
                if (lerpTotalTime < lerpDuration )
                {
                    currentWidth = Mathf.LerpUnclamped(HealthRect.sizeDelta.x, targerWidth, lerpTotalTime / lerpDuration);
                    HealthRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);

                    lerpTotalTime += Time.deltaTime;
                }
                else
                {
                    if (HealthDecreasedRect != null)
                    {
                        HealthDecreasedRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targerWidth);
                    }

                    HealthRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targerWidth);

                    lerpTotalTime = 0;
                    isLerp = false;
                }
            }
        }

        public void ChangeHealthBar(float currentHealth, float maxHealth, float valueBeforeChange)
        {
            this.currentHealth = currentHealth;
            this.maxHealth = maxHealth;

            targerWidth = currentHealth / maxHealth * originalWidth;

            isLerp = true;
            if (healthTextMesh != null)
            {
                healthTextMesh.text = $"{currentHealth} / {maxHealth}";
            }

            UpdateHealthBarColor();
        }
        
        private void UpdateHealthBarColor()
        {
            if (healthToColors.Length > 0)
            {
                foreach (HealthToColor healthToColor in healthToColors)
                {
                    if (currentHealth >= maxHealth * healthToColor.range[0] && currentHealth <= maxHealth * healthToColor.range[1])
                    {
                        if (currentHealthBarColor != healthToColor.color)
                        {
                            currentHealthBarColor = healthToColor.color;
                            healthBarImage.color = currentHealthBarColor;
                            break;
                        }
                    }
                }
            }
        }

        private void SwitchHealthTextVisible()
        {
            if (healthTextMesh != null)
            {
                if (currentHealth == maxHealth)
                {
                    if (healthTextMesh.gameObject.activeSelf)
                    {
                        healthTextMesh.gameObject.SetActive(false);
                    }
                }

                if (isHover)
                {
                    healthTextMesh.gameObject.SetActive(true);
                }
            }
        }

        public void HandleHover()
        {
            isHover = true;
        }

        public void HandleExit()
        {
            isHover = false;
        }
    }
}
