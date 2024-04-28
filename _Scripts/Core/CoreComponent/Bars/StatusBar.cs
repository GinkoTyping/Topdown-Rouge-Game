using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class StatusBar :CoreComponent
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
        private Stats stats;

        private bool flipped = false;

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