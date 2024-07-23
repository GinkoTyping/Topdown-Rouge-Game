using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedPrefabInstantiator : MonoBehaviour
{
    [Header("Buff")]
    [SerializeField] private GameObject healingBuff;

    public static SharedPrefabInstantiator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject InstantiateHealingParticle(Transform rect, string name = "")
    {
        GameObject healingParticle;

        Transform existTransform = null;

        if (name != "")
        {
            existTransform = rect.Find(name);
        }

        if (existTransform == null)
        {
            healingParticle = Instantiate(healingBuff, rect);

        } else
        {
            healingParticle = existTransform.gameObject;
            if (!healingParticle.activeSelf)
            {
                healingParticle.SetActive(true);
            }
        }
        
        if (healingParticle != null && name != "")
        {
            healingParticle.name = name;
        }

        return healingParticle;
    }
}
