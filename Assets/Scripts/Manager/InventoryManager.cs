using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory DataBase", menuName = "DataBase/Inventory")]
public class InventoryDataBase : ScriptableObject
{
    //public List<InventorySlot> Container = new List<InventorySlot>();
    public List<InventorySlot> weaponList = new List<InventorySlot>();
    public List<InventorySlot> armorList = new List<InventorySlot>();
    public List<InventorySlot> consumList = new List<InventorySlot>();

    public void AbbItem(SOItem _item, int _amount)
    {
        if(_item.Type == ItemType.Weapon)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (weaponList[i].item == null)
                {
                    weaponList[i].item = _item;
                    weaponList[i].TrySetItem(_item);
                }
            }
        }
        if (_item.Type == ItemType.Armor)
        {
            for (int i = 0; i < armorList.Count; i++)
            {
                if (armorList[i].item == null)
                {
                    armorList[i].item = _item;
                    armorList[i].TrySetItem(_item);
                }
            }
        }
        if (_item.Type == ItemType.Consum)
        {
            for (int i = 0; i < consumList.Count; i++)
            {
                if (consumList[i].item == null)
                {
                    consumList[i].item = _item;
                    consumList[i].TrySetItem(_item);
                }
                else if (consumList[i].item == _item)
                {
                    consumList[i].amount += _amount;
                }
            }
        }
    }
}

public class InventoryManager : MonoBehaviour
{
    public InventoryUI inventoryUI;

    public RectTransform weaponContent;
    public RectTransform armorContent;
    public RectTransform consumContent;

    //private List<InventorySlot> weaponList = new List<InventorySlot>();
    //private List<InventorySlot> armorList = new List<InventorySlot>();
    //private List<InventorySlot> consumList = new List<InventorySlot>();

    public InventoryDataBase dataBase;

    public InventorySlot focusSlot = null;
    public InventorySlot selectedSlot = null;


    private void Start()
    {
        dataBase = GameManager.Resource.Load<InventoryDataBase>("UI/PopUpUI/Inventory/PlayerInventory");

        GameManager.UI.StartSetPopUpUI<InventoryUI>("UI/PopUpUI/Inventory/Inventory");
        GameManager.UI.StartSetPopUpUI<EquipUI>("UI/PopUpUI/Equip/EquipUI");
        GameManager.UI.EndSetPopUpUI();
    }

    public void MakeSlotParent(InventoryUI inventory)
    {
        weaponContent =
            GameManager.Resource.Instantiate<RectTransform>("UI/PopUpUI/Inventory/ItemList");
        weaponContent.gameObject.name = "WeaponList";
        MakeSlotsList(dataBase.weaponList, weaponContent);
        weaponContent.SetParent(inventory.showItemArea, false);

        armorContent =
            GameManager.Resource.Instantiate<RectTransform>("UI/PopUpUI/Inventory/ItemList");
        armorContent.gameObject.name = "ArmorList";
        MakeSlotsList(dataBase.armorList, armorContent);
        armorContent.SetParent(inventory.showItemArea, false);

        consumContent =
            GameManager.Resource.Instantiate<RectTransform>("UI/PopUpUI/Inventory/ItemList");
        consumContent.gameObject.name = "ConsumList";
        MakeSlotsList(dataBase.consumList, consumContent);
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
        if (item.Type == ItemType.Weapon)
        {
            emptySlot = dataBase.weaponList.Find((x) => { return x.item == null; });
            if (emptySlot == null)
            {
                return false;
            }
        }
        else if(item.Type == ItemType.Armor)
        {
            emptySlot = dataBase.armorList.Find((x) => { return x.item == null; });
            if (emptySlot == null)
            {
                return false;
            }
        }
        else if (item.Type == ItemType.Consum)
        {
            emptySlot = dataBase.consumList.Find((x) => { return x.item == null; });
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
