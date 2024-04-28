using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ginko.Weapons.Components
{
    public class WeaponGenerator : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] WeaponDataSO data;
        private List<WeaponComponent> componentAlreadyOnWeapon = new List<WeaponComponent>();
        private List<WeaponComponent> componentAddedToWeapon = new List<WeaponComponent>();
        private List<Type> componentDependencies = new List<Type>();

        private Animator anim;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }
        private void Start()
        {
            GenerateWeapon(data);
        }
        public void GenerateWeapon(WeaponDataSO weaponData)
        {
            weapon.SetData(weaponData);

            componentAlreadyOnWeapon.Clear();
            componentAddedToWeapon.Clear();
            componentDependencies.Clear();

            componentAlreadyOnWeapon = GetComponents<WeaponComponent>().ToList();
            componentDependencies = weaponData.GetAllDependencies();

            // 添加需要的
            foreach (var dependency in componentDependencies)
            {

                if (componentAddedToWeapon.FirstOrDefault(component => component.GetType() == dependency))
                {
                    continue;
                }
                var weaponComponent = componentAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);
                if (weaponComponent == null)
                {
                    weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;
                }

                weaponComponent.Init();
                componentAddedToWeapon.Add(weaponComponent);
            }

            // 删除冗余的
            var componentsToMove = componentAlreadyOnWeapon.Except(componentAddedToWeapon);
            foreach (var component in componentsToMove)
            {
                Destroy(component);
            }

            anim.runtimeAnimatorController = data.AnimatorController;
        }
    }
}