using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Item/Weapon")]
public class WeaponItem : SOItem
{
    public int attackBonus;
    private void Awake()
    {
        Type = ItemType.Weapon;
    }
}
