using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Shared.Utilities
{
    public class PoolHelper : MonoBehaviour
    {
        [SerializeField] private GameObject defaultPool;

        public PoolManager GetPoolByPrefab(Transform poolContainer, GameObject prefab)
        {
            PoolManager poolManager;

            Transform poolTransform = poolContainer.Find($"{prefab.name}Pool");
            if (poolTransform == null)
            {
                poolManager = Instantiate(defaultPool, poolContainer).GetComponent<PoolManager>();
                poolManager.gameObject.name = $"{prefab.name}Pool";

                poolManager.SetCurrrentObject(prefab);
                poolManager.SetCurrentParrent(poolContainer);
            }
            else
            {
                poolManager = poolTransform.GetComponent<PoolManager>();
            }

            return poolManager;
        }
    }
}