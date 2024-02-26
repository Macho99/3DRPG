using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

[Serializable]
public enum ItemID
{
    Katana,
    NomalArmor,
    Bow,
    Apple,
}
public enum ItemType
{
    Weapon,
    Consum,
    Armor,
}

public class Item
{
    public ItemID itemID;
    public ItemType itemType;
    public Sprite itemIcon;
    public string itemName;
    public string itemExplain;
    public string itemStatus;

    public GameObject itemPrefab;

    public Item DeepCopy()
    {
        Item copy = new Item();
        copy.itemID = this.itemID;
        copy.itemType = this.itemType;

        return copy;
    }
}

[CreateAssetMenu]
public class SOItem : ScriptableObject
{

}