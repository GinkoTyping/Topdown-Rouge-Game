using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTitle : MonoBehaviour
{
    [SerializeField]
    private bool isResource;
    [SerializeField]
    private ResourceType resourceType;
    [SerializeField]
    private AttributeType attributeType;

    private TextMeshProUGUI textMesh;
    private PlayerStats playerStats;

    private List<AttributeType> multiAttributes = new List<AttributeType> { 
        AttributeType.CriticalChance,
        AttributeType.CriticalDamage,
        AttributeType.DamageReduction,
        AttributeType.MoveSpeed
        };

    private void Awake()
    {
        textMesh = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        playerStats = Player.Instance.Core.GetCoreComponent<PlayerStats>();
    }

    private void Update()
    {
        if (isResource)
        {
            ResourceStat resouceStat = playerStats.GetAttribute(resourceType);
            textMesh.text = $"{resouceStat.CurrentValue} / {resouceStat.MaxValue}";
        } else
        {
            float value = playerStats.GetAttribute(attributeType).CurrentValue;
            textMesh.text = value.ToString();
            if (multiAttributes.Contains(attributeType))
            {
                textMesh.text = $"{Mathf.Round(value * 100)}%";
            }
        }
    }
}
