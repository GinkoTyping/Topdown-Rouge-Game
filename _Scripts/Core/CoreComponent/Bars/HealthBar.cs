using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Ginko.CoreSystem
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private float lerpDuration;
        private float lerpTotalTime;
        private bool isLerp;

        private RectTransform HealthRect;
        private RectTransform HealthDecreasedRect;

        private float originalWidth;
        private float currentWidth;
        private float targerWidth = -1f;

        protected void Awake()
        {
            HealthRect = transform.Find("Health").GetComponent<RectTransform>();
            HealthDecreasedRect = transform.Find("Health_Decreased")?.GetComponent<RectTransform>();

            originalWidth = HealthRect.sizeDelta.x;
        }

        private void Update()
        {
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

        public void ChangeHealthBar(float currentHealth, float maxHealth)
        {
            targerWidth = currentHealth / maxHealth * originalWidth;
            isLerp = true;
        }
    }
}
