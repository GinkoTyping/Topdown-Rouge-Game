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

        private HealthBar healthBar;

        private bool flipped = false;

        protected override void Awake()
        {
            base.Awake();

            healthBar = GetComponentInChildren<HealthBar>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            Stats.Health.OnCurrentValueChange += healthBar.ChangeHealthBar;
        }

        private void OnDisable()
        {
            Stats.Health.OnCurrentValueChange -= healthBar.ChangeHealthBar;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if ((Movement.FacingDirection < 0 && !flipped) || Movement.FacingDirection > 0 && flipped)
            {
                transform.Rotate(0, 180, 0);
                flipped = !flipped;
            }
        }
    }
}