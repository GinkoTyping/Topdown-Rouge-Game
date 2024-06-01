using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    [Serializable]
    public abstract class BaseStat
    {
        [field: SerializeField] public float MaxValue { get; private set; }

        public event Action OnCurrentValueZero;
        public event Action<float, float> OnCurrentValueChange;

        private float currentValue;
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

        public void Increase(float amount) => CurrentValue += amount;
        public void Decrease(float amount) => CurrentValue -= amount;
    }
}
