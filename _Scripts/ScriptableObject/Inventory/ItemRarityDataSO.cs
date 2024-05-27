using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemRarityData", menuName = "Data/Inventory/Rarity Data")]
public class ItemRarityDataSO : ScriptableObject
{
    public string text;

    [SerializeField]
    public Material[] rarityMaterialList;
}
