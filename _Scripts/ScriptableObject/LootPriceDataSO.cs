using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLootPriceData", menuName = "Data/Loot/Price Data")]
public class LootPriceDataSO : ScriptableObject
{
    public PriceByTile[] priceList;
}

[Serializable]
public class PriceByTile
{
    public Rarity rarity;
    public int price;
}
