using Ginko.StateMachineSystem;
using System;
using System.Linq;
using UnityEngine;
using Ginko.CoreSystem;
using Ginko.Weapons;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace Ginko.PlayerSystem
{
    public class Player : Entity
    {
        public static Player Instance { get; private set; }
        public Vector2 MoveDirection { get; private set; }
        public bool IsAttackInput { get; private set; }
        public PlayerInputEventHandler InputHandler { get; private set; }
        public PlayerInput InputAction;
        private Weapon primaryWeapon;
        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            InputHandler = GetComponent<PlayerInputEventHandler>();
            primaryWeapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
            primaryWeapon.SetCore(Core);
            InputAction = GetComponent<PlayerInput>();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            base.Update();

            MoveDirection = InputHandler.Direction;
            IsAttackInput = InputHandler.PrimaryAttack;
        }

        protected override void InitiateStateMachine()
        {
            StateMachine.Initialize(IdleState);
        }

        protected override void InitiateBasicStates()
        {
            IdleState = new P_IdleState(this, StateMachine);
            MoveState = new P_MoveState(this, StateMachine);
            MeleeAttackState = new P_AttackState(this, StateMachine, primaryWeapon);
            DeathState = new DeathState(this, StateMachine);
        }

        private void OnDisable()
        {
            StateMachine.ChangeState(IdleState);
        }
    }
}
