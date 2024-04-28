using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    [Serializable]
    public class Stat
    {

        [field: SerializeField] public float MaxValue { get; private set; }
        [field: SerializeField] public float RecoveryRate { get; private set; }
        public event Action OnCurrentValueZero;
        public event Action<float, float> OnCurrentValueChange;
        public float CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = Mathf.Clamp(value, 0f, MaxValue);
                if (currentValue <= 0)
                {
                    OnCurrentValueZero?.Invoke();
                }

                OnCurrentValueChange?.Invoke(currentValue, MaxValue);
            }
        }
        private float currentValue;
        public void Init() => CurrentValue = MaxValue;
        public void Increase(float amount) => CurrentValue += amount;
        public void Decrease(float amount) => CurrentValue -= amount;

        public void LogicUpdate()
        {
            if (RecoveryRate > 0 && CurrentValue < MaxValue)
            {
                Increase(RecoveryRate * Time.deltaTime);
            }
        }
    }
}
