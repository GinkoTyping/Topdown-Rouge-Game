using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class StatusBar : CoreComponent
    {
        public Movement Movement
        {
            get => movement ??= Core.GetCoreComponent<Movement>();
        }
        private Movement movement;

        public Stats Stats
        {
            get => stats ??= Core.GetCoreComponent<Stats>();
        }
        public Stats stats { get; private set; }

        private Death death;
        private HealthBar healthBar;

        protected override void Awake()
        {
            base.Awake();

            healthBar = GetComponentInChildren<HealthBar>();
        }

        private void Start()
        {
            gameObject.SetActive(true);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            gameObject.SetActive(true);
            death = Core.GetCoreComponent<Death>();

            Stats.Health.OnCurrentValueChange += healthBar.ChangeHealthBar;
            death.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            Stats.Health.OnCurrentValueChange -= healthBar.ChangeHealthBar;
            death.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }
    }
}