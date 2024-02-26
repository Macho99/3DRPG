using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryUI inventoryUI;

    public RectTransform weaponContent;
    public RectTransform armorContent;
    public RectTransform consumContent;

    private List<InventorySlot> weaponList = new List<InventorySlot>();
    private List<InventorySlot> armorList = new List<InventorySlot>();
    private List<InventorySlot> consumList = new List<InventorySlot>();

    public InventorySlot focusSlot = null;
    public InventorySlot selectedSlot = null;


    private void Start()
    {
        GameManager.UI.StartSetPopUpUI<InventoryUI>("UI/PopUpUI/Inventory/Inventory");
        GameManager.UI.StartSetPopUpUI<EquipUI>("UI/PopUpUI/Equip/EquipUI");
        GameManager.UI.EndSetPopUpUI();
    }

    public void MakeSlotParent(InventoryUI inventory)
    {
        weaponContent =
            GameManager.Resource.Instantiate<RectTransform>("UI/PopUpUI/Inventory/ItemList");
        weaponContent.gameObject.name = "WeaponList";
        MakeSlotsList(weaponList, weaponContent);
        weaponContent.SetParent(inventory.showItemArea, false);

        armorContent =
            GameManager.Resource.Instantiate<RectTransform>("UI/PopUpUI/Inventory/ItemList");
        armorContent.gameObject.name = "ArmorList";
        MakeSlotsList(armorList, armorContent);
        armorContent.SetParent(inventory.showItemArea, false);

        consumContent =
            GameManager.Resource.Instantiate<RectTransform>("UI/PopUpUI/Inventory/ItemList");
        consumContent.gameObject.name = "ConsumList";
        MakeSlotsList(consumList, consumContent);
        consumContent.SetParent(inventory.showItemArea, false);
    }

    private void MakeSlotsList(List<InventorySlot> parentList, RectTransform parentTrans)
    {
        for (int i = 0; i < 10; i++)
        {
            var makeSlot = GameManager.Resource.Instantiate<InventorySlot>("UI/PopUpUI/Inventory/Slot", parentTrans.transform);
            makeSlot.name = $"{parentTrans.gameObject.name}_{i}";
            parentList.Add(makeSlot);
            if(parentTrans.gameObject.name == "WeaponList")
            {
                makeSlot.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
            if (parentTrans.gameObject.name == "ArmorList")
            {
                makeSlot.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }
    }

    public bool TryGainItem(SOItem item)
    {
        InventorySlot emptySlot;
        if (item.itemType == ItemType.Weapon)
        {
            emptySlot = weaponList.Find((x) => { return x.item == null; });
            if (emptySlot == null)
            {
                return false;
            }
        }
        else if(item.itemType == ItemType.Armor)
        {
            emptySlot = armorList.Find((x) => { return x.item == null; });
            if (emptySlot == null)
            {
                return false;
            }
        }
        else if (item.itemType == ItemType.Consum)
        {
            emptySlot = consumList.Find((x) => { return x.item == null; });
            if (emptySlot == null)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return emptySlot.TrySetItem(item);
    }
}
