using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab;

    public InventoryUI inventoryUI;

    public InventoryObject invenData;

    public InventorySlot focusSlot = null;
    public InventorySlot selectedSlot = null;

    private void Awake()
    {
        slotPrefab = GameManager.Resource.Load<GameObject>("UI/PopUpUI/Inventory/Slot");
        inventoryUI = GameManager.Resource.Load<InventoryUI>("UI/PopUpUI/Inventory/InventoryDisplay");
        //invenData = GameManager.Resource.Load<InventoryObject>("UI/PopUpUI/Inventory/Player");
    }


    private void Start()
    {

    }

    private void Update()
    {
        
    }
}
