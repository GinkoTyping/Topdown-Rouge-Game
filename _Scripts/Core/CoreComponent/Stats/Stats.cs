using System;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Stats : CoreComponent
    {
        [field: SerializeField] public Stat Health {  get; private set; }
        [field: SerializeField] public Stat Poise {  get; private set; }

        private void OnEnable()
        {
            Health.Init();
            Poise.Init();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Health.LogicUpdate();
            Poise.LogicUpdate();
        }
    }
}

