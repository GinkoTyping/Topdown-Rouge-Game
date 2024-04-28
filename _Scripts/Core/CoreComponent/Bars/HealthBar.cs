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
        private Stats stats;

        private GameObject HealthGO;
        private Image HealthImage;

        protected void Awake()
        {
            HealthGO = transform.Find("Health").gameObject;
            HealthImage = HealthGO.GetComponent<Image>();
        }
        private void ChangeHealthBar(float currentHealth, float maxHealth)
        {
            HealthImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHealth / maxHealth);
        }

        private void Start()
        {
            stats = transform.parent.GetComponent<StatusBar>().Stats;

            stats.Health.OnCurrentValueChange += ChangeHealthBar;
        }

        private void OnDisable()
        {
            stats.Health.OnCurrentValueChange -= ChangeHealthBar;
        }
    }
}
