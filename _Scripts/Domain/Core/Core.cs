using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Ginko.Data;
using Ginko.StateMachineSystem;

namespace Ginko.CoreSystem
{
    public class Core : MonoBehaviour
    {
        private List<CoreComponent> coreComponents = new List<CoreComponent>();

        public void LogicUpdate()
        {
            foreach (CoreComponent component in coreComponents)
            {
                component.LogicUpdate();
            }
        }

        public void AddComponent(CoreComponent component)
        {
            if (!coreComponents.Contains(component))
            {
                coreComponents.Add(component);
            }
        }

        public T GetCoreComponent<T>() where T : CoreComponent
        {
            var comp = coreComponents.OfType<T>().FirstOrDefault();
            if (comp == null)
            {
                return GetComponentInChildren<T>();
            }
            return comp;
        }
    }

}
