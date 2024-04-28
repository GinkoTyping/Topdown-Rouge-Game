using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class InputHold : WeaponComponent
    {
        private Animator anim;
        private bool input;
        private bool minHoldPassed;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            minHoldPassed = false;
        }
        private void SetAnimParameter()
        {
            if (input) 
            { 
                anim.SetBool("hold", input);
            }
            else if (minHoldPassed)
            {
                anim.SetBool("hold", input);
            }

        }
        private void HandleCurrentInputChange(bool newInput)
        {
            input = newInput;
            SetAnimParameter();
        }
        private void HandleMinHoldPassed()
        {
            minHoldPassed = true;
            SetAnimParameter();
        }
        protected override void Awake()
        {
            base.Awake();

            anim = GetComponentInChildren<Animator>();
        }
        protected override void Start()
        {
            base.Start();

            weapon.OnCurrentInputChange += HandleCurrentInputChange;
            EventHandler.OnMinHoldPassed += HandleMinHoldPassed;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            weapon.OnCurrentInputChange -= HandleCurrentInputChange;
            EventHandler.OnMinHoldPassed -= HandleMinHoldPassed;
        }
    }
}