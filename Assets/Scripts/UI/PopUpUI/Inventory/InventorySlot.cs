using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Inventory DataBase", menuName = "DataBase/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> weaponList = new List<InventorySlot>();
    public List<InventorySlot> armorList = new List<InventorySlot>();
    public List<InventorySlot> consumList = new List<InventorySlot>();

    public void AbbItem(SOItem _item, int _amount)
    {
        if (_item.Type == ItemType.Weapon)
        {
            bool hasItem = false;

            for (int i = 0; i < weaponList.Count; i++)
            {
                if (weaponList[i].item == _item)
                {
                    weaponList[i].item = _item;
                    hasItem = true;
                    break;
                }
            }
            if (!hasItem)
            {
                weaponList.Add(new InventorySlot(_item,_amount));
            }
        }
        if (_item.Type == ItemType.Armor)
        {
            bool hasItem = false;

            for (int i = 0; i < armorList.Count; i++)
            {
                if (armorList[i].item == null)
                {
                    armorList[i].item = _item;
                    hasItem = true;
                    break;
                }
            }
            if(!hasItem)
            {
                armorList.Add(new InventorySlot(_item,_amount));
            }
        }
        if (_item.Type == ItemType.Consum)
        {
            bool hasItem = false;
            for (int i = 0; i < consumList.Count; i++)
            {
                if (consumList[i].item == null)
                {
                    consumList[i].item = _item;
                    break;
                }
                else if (consumList[i].item == _item)
                {
                    consumList[i].amount += _amount;
                    hasItem = true;
                    break;
                }
            }
            if(!hasItem)
            {
                consumList.Add(new InventorySlot(_item,_amount));
            }
        }
    }
}

[System.Serializable]
public class InventorySlot : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SOItem item;
    public int amount;
    public Sprite sprite;

    public InventorySlot(SOItem _item, int _amount)
    {
        item = _item;
        amount = _amount;
        sprite = _item.Icon;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Inven.focusSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Inven.focusSlot = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if( item == null) 
        {
            Debug.Log("Click null");
        }
        else if (item != null)
        {
            Debug.Log($"Click {item.Name}");
            GameManager.Inven.selectedSlot = this;
        }
    }
}

