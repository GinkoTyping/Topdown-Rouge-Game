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
        [field: SerializeField] public float MinValue { get; private set; }
        [field: SerializeField] public float InitValue { get; private set; }

        public event Action OnCurrentValueZero;
        public event Action<float, float, float> OnCurrentValueChange;

        private float currentValue = 0;
        public float CurrentValue
        {
            get => currentValue;
            set
            {
                float valueBeforeChange = currentValue;
                currentValue = Mathf.Clamp(value, MinValue, MaxValue);
                if (currentValue <= 0)
                {
                    OnCurrentValueZero?.Invoke();
                }

                OnCurrentValueChange?.Invoke(currentValue, MaxValue, valueBeforeChange);
            }
        }

        public virtual void Increase(float amount) => CurrentValue += amount;

        public virtual void Decrease(float amount) => CurrentValue -= amount;

        public void ChangeMaxValue(float value)
        {
            MaxValue += value;

            OnCurrentValueChange?.Invoke(currentValue, MaxValue, currentValue);
        }
    }
}
