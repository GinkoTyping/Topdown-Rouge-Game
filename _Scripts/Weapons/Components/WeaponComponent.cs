using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        protected Weapon weapon;
        protected AnimationEventHandler EventHandler;
        protected Core Core => weapon.Core;
        protected bool isAttackActive;

        protected virtual void Awake()
        {
            weapon = GetComponent<Weapon>();
            EventHandler = GetComponentInChildren<AnimationEventHandler>();
        }

        protected virtual void Start()
        {
            weapon.OnEnter += HandleEnter;
            weapon.OnExit += HandleExist;
        }

        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }
        protected virtual void HandleExist()
        {
            isAttackActive = false;
        }

        protected virtual void OnDestroy()
        {
            weapon.OnEnter -= HandleEnter;
            weapon.OnExit -= HandleExist;
        }
        public virtual void Init()
        {

        }
    }

    public abstract class WeaponComponent<T1, T2>: WeaponComponent where T1 : ComponentData<T2> where T2 : AttackData
    {
        protected T1 data;
        protected T2 currentAttackData;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentAttackData = data.AttackData[weapon.CurrentAttackCounter];
        }
        public override void Init()
        {
            base.Init();

            data = weapon.Data.GetData<T1>();
        }
    }
}
