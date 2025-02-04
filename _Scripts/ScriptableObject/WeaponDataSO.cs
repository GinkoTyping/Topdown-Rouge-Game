using System;
using System.Collections.Generic;
using UnityEngine;
using Ginko.Weapons.Components;
using System.Linq;

namespace Ginko.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic Weapon Data", order = 0)]
    public class WeaponDataSO : ScriptableObject
    {
        [field: SerializeField] public RuntimeAnimatorController AnimatorController {  get; private set; }
        [field: SerializeField] public int NumberOfAttacks {  get; private set; }

        [field: SerializeReference] public List<ComponentData> ComponentData {  get; private set; }

        public T GetData<T>()
        {
            return ComponentData.OfType<T>().FirstOrDefault();
        }
        public List<Type> GetAllDependencies()
        {
            return ComponentData.Select(x => x.ComponentDependency).ToList();
        }

        public void AddData(ComponentData data)
        {
            if (ComponentData.FirstOrDefault(t => t.GetType() == data.GetType()) == null)
            {
                ComponentData.Add(data);
            } 
        }
    }
}

