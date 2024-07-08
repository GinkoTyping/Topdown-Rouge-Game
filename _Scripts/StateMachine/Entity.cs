using Ginko.CoreSystem;
using Ginko.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace Ginko.StateMachineSystem
{
    public abstract class Entity : MonoBehaviour
    {
        public Animator Anim { get; private set; }
        public Core Core { get; private set; }

        public Movement Movement
        {
            get => movement ??= Core.GetCoreComponent<Movement>();
        }
        private Movement movement;
        public Detections Detections
        {
            get => detections ??= Core.GetCoreComponent<Detections>();
        }
        private Detections detections;
        public Pathfinding Pathfinding
        {
            get => pathfinding ??= Core.GetCoreComponent<Pathfinding>();
        }
        private Pathfinding pathfinding;

        public Death Death
        {
            get => death ??= Core.GetCoreComponent<Death>();
        }
        private Death death;

        public IdleState IdleState;
        public MoveState MoveState;
        public AttackState MeleeAttackState;
        public AttackState RangedAttackState;
        public HostileDetectedState HostileDetectedState;
        public DeathState DeathState;

        public FiniteStateMachine StateMachine { get; private set; }

        public ObjectPool<GameObject> pool {  get; private set; }

        [SerializeField]
        public EntityDataSO EntityData;


        protected virtual void Awake()
        {
            Anim = GetComponent<Animator>();
            Core = GetComponentInChildren<Core>();

            StateMachine = new FiniteStateMachine();
            ;
        }
        protected virtual void Start()
        {
            InitiateBasicStates();
            InitiateStateMachine();
        }

        protected virtual void Update()
        {
            Core.LogicUpdate();

            StateMachine.CurrentState.LogicUpdate();
            StateMachine.CurrentState.AfterLogicUpdate();
        }

        protected virtual void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        protected abstract void InitiateStateMachine();
        protected abstract void InitiateBasicStates();

        public void SetPool(ObjectPool<GameObject> pool)
        {
            this.pool = pool;
        }
    }
}
