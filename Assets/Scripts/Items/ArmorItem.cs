using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Item/Armor")]
public class ArmorItem : SOItem
{
    public int defenceBonus;
    private void Awake()
    {
        Type = ItemType.Armor;
    }
}
