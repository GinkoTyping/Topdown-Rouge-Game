using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    [Serializable]
    public abstract class ComponentData
    {
        [SerializeField, HideInInspector] private string name;
        public Type ComponentDependency {  get; protected set; }
        public ComponentData()
        {
            SetComponentName();
            SetComponentDependency();
        }

        // 子类必须实现父类中的抽象方法
        protected abstract void SetComponentDependency();
        public void SetComponentName() => name = GetType().Name;
        public virtual void SetAttackDataNames() { }
        public virtual void InitializeAttackData(int numberOfAttacks) { }
    }

    [Serializable]
    public abstract class ComponentData<T> : ComponentData where T : AttackData
    {
        [SerializeField] private T[] attackData;
        public T[] AttackData { get => attackData; private set => attackData = value; }

        public override void SetAttackDataNames()
        {
            base.SetAttackDataNames();

            for (int i = 0; i < AttackData.Length; i++)
            {
                AttackData[i].SetAttackName(i + 1);
            }
        }
        public override void InitializeAttackData(int numberOfAttacks)
        {
            base.InitializeAttackData(numberOfAttacks);

            int oldLength = AttackData != null ? AttackData.Length : 0;
            if (oldLength != numberOfAttacks)
            {
                Array.Resize(ref attackData, numberOfAttacks);
            }
            if (oldLength < numberOfAttacks)
            {
                for (int i = oldLength; i < attackData.Length; i++)
                {
                    attackData[i] = Activator.CreateInstance(typeof(T)) as T;
                }
            }
            SetAttackDataNames();
        }
    }

}
