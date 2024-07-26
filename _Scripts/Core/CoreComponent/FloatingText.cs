using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class FloatingText : CoreComponent
    {
        [SerializeField] private Vector2 defaultOffset = new Vector2 (0.6f, 0);

        private PoolManager poolManager;
        private Stats stats;
        private Movement movement;

        protected override void Awake()
        {
            base.Awake();

            poolManager = GetComponent<PoolManager>();
        }

        private void Start()
        {
            stats = Core.GetCoreComponent<Stats>();
            movement = Core.GetCoreComponent<Movement>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (stats == null)
            {
                stats = Core.GetCoreComponent<Stats>();
            }

            stats.Health.OnCurrentValueChange += HandleHealthChange;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

        }

        private void HandleHealthChange(float currentValue, float maxValue, float valueBeforeChange)
        {
            float changedValue = valueBeforeChange - currentValue;
            if (changedValue == 0)
            {
                return;
            }

            bool isAdd = changedValue < 0;
            Color color = isAdd ? Color.green : Color.red;
            string indicator = isAdd ? "+" : "-";
            string content = $"{indicator}{Mathf.Abs(changedValue)}";

            GameObject textGO = poolManager.Pool.Get();
            textGO.GetComponent<AnimatedText>().Init(
                poolManager,
                defaultOffset,
                content,
                color);
        }
    }
}