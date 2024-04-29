using Ginko.Data;
using UnityEngine;
using Ginko.StateMachineSystem;


namespace Ginko.CoreSystem
{
    public class CoreComponent: MonoBehaviour
    {
        protected Core Core { get; private set; }
        protected EntityDataSO entityData;
        public virtual void LogicUpdate() { }
        protected virtual void Awake()
        {
            Core = transform.parent.GetComponent<Core>();
            Core.AddComponent(this);
        }
    }
}
