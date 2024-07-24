using Ginko.Data;
using UnityEngine;
using Ginko.StateMachineSystem;


namespace Ginko.CoreSystem
{
    public class CoreComponent: MonoBehaviour
    {
        public Core Core { get; private set; }
        
        public virtual void OnEnable() { }
        public virtual void LogicUpdate() { }
        protected virtual void Awake()
        {
            Core = transform.parent.GetComponent<Core>();
            Core.AddComponent(this);
        }
    }
}
