using Ginko.Weapons.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Ginko.Weapons
{
    [CustomEditor(typeof(WeaponDataSO))]
    public class WeaponDataSOEditor : Editor
    {
        private static List<Type> dataComponentTypes = new List<Type>();
        private WeaponDataSO dataSO;
        private bool showForceUpdateButtons;
        private bool showAddComponentButtons;
        private void OnEnable()
        {
            dataSO = (WeaponDataSO)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            showAddComponentButtons = EditorGUILayout.Foldout(showAddComponentButtons, "Add Components");
            if (showAddComponentButtons)
            {
                foreach (var type in dataComponentTypes)
                {
                    if (GUILayout.Button(type.Name))
                    {
                        var comp = (ComponentData)Activator.CreateInstance(type);

                        comp.InitializeAttackData(dataSO.NumberOfAttacks);

                        dataSO.AddData(comp);

                        // 即使ComponentData的数据没有变化或者为空，也保存
                        EditorUtility.SetDirty(dataSO);
                    }
                }
            }

            showForceUpdateButtons = EditorGUILayout.Foldout(showForceUpdateButtons, "Force Update");
            if (showForceUpdateButtons)
            {
                if (GUILayout.Button("Set Number of Attacks"))
                {
                    foreach (var item in dataSO.ComponentData)
                    {
                        item.InitializeAttackData(dataSO.NumberOfAttacks);
                    }
                }
                if (GUILayout.Button("Force Update Component Names"))
                {
                    foreach (var item in dataSO.ComponentData)
                    {
                        item.SetComponentName();
                    }
                }
            }
        }

        [DidReloadScripts]
        private static void OnRecompile()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());
            var filterTypes = types.Where(
                type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters && type.IsClass
            );

            dataComponentTypes = filterTypes.ToList();
        }
    }
}
