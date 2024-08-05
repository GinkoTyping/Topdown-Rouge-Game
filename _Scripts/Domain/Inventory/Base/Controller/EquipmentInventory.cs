using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject equipmentPage;
    [SerializeField]
    private GameObject equipmentDetailPage;
    [SerializeField]
    private TextMeshProUGUI switchEquipemntPageButton;

    private bool isShowEquipmentPage = true;

    public EquipmentSlot selectedEquipmentSlot { get; private set; }
    public EquipmentSlot[] equipmentSlots;

    private void Start()
    {
        equipmentSlots = equipmentPage.GetComponentsInChildren<EquipmentSlot>();
    }

    public void SetSelectedEquipmentSlot(EquipmentSlot equipmentSlot)
    {
        selectedEquipmentSlot = equipmentSlot;
    }

    public void OnSwitchEquipmentPage()
    {
        isShowEquipmentPage = !isShowEquipmentPage;

        equipmentPage.SetActive(isShowEquipmentPage);
        equipmentDetailPage.SetActive(!isShowEquipmentPage);

        switchEquipemntPageButton.text = isShowEquipmentPage ? "Detailed Stauts" : "Equipment Status";

        SoundManager.Instance.ButtonClick();
    }
}
