using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySound : MonoBehaviour
{
    [Header("Inventory Audio")]
    [SerializeField]
    public AudioClip equipmentAudio;
    [SerializeField]
    public AudioClip treasureAudio;
    [SerializeField]
    public AudioClip consumableAudio;
    [SerializeField]
    public AudioClip removeAudio;
    [SerializeField]
    public AudioClip selectAudio;
    [SerializeField]
    public AudioClip equipAudio;

    public void PlayPlaceItemAudio(InventoryItem item)
    {
        ItemType itemType = item.data.itemType;
        if (itemType == ItemType.Treasure)
        {
            SoundManager.Instance.PlaySound(treasureAudio);
        }
        else if (itemType == ItemType.Consumable)
        {
            SoundManager.Instance.PlaySound(consumableAudio);
        }
        else if (itemType == ItemType.Equipment)
        {
            SoundManager.Instance.PlaySound(equipmentAudio);
        }
    }

    public void PlayRemoveItemAudio()
    {
        SoundManager.Instance.PlaySound(removeAudio);
    }

    public void PlayPickUpItemAudio()
    {
        SoundManager.Instance.PlaySound(selectAudio);
    }

    public void PlayEquipItemAudio()
    {
        SoundManager.Instance.PlaySound(equipAudio);
    }
}
