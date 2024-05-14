using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    private InventoryController inventoryController;
    void Start()
    {
        inventoryController = GameObject.Find("UI").GetComponent<InventoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickButton()
    {
        inventoryController.Test();
    }
}
