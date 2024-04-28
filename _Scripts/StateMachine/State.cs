using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Ginko.StateMachineSystem
{
    public abstract class State
    {
        protected Entity Entity { get; private set; }
        protected FiniteStateMachine StateMachine { get; private set; }
        public AnimBoolName AnimBoolName;
        public bool IsAnimationFinished;
        protected float StartTime { get; private set; }

        public State(Entity entity, FiniteStateMachine stateMachine)
        {
            Entity = entity;
            StateMachine = stateMachine;
            SetAnimBoolName();
        }
        protected abstract void SetAnimBoolName();
        public virtual void Enter()
        {
            StartTime = Time.time;
            IsAnimationFinished = false;

            Entity.Anim.SetBool(AnimBoolName.ToString(), true);
            DoChecks();
            RegisterEvents();
        }
        public virtual void Exit()
        {
            Entity.Anim.SetBool(AnimBoolName.ToString(), false);
            UnRegisterEvents();
        }
        public virtual void LogicUpdate() { }
        public virtual void AfterLogicUpdate() { }
        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }

        public virtual void DoChecks() { }

        public virtual void RegisterEvents() { }
        public virtual void UnRegisterEvents() { }
    }

    public enum AnimBoolName
    {
        Idle,
        Move,
        Attack,
        HostileDetected
    }
}
