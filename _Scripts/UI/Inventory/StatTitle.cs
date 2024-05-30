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
            textMesh.text = $"{playerStats.GetAttribute(resourceType).CurrentValue} / {playerStats.GetAttribute(resourceType).MaxValue}";
        } else
        {
            textMesh.text = playerStats.GetAttribute(attributeType).CurrentValue.ToString();
        }
    }
}
