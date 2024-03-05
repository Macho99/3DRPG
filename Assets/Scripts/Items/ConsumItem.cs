using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consum Item", menuName = "Item/Consum")]
public class ConsumItem : SOItem
{
    public int HealthValue;
    private void Awake()
    {
        Type = ItemType.Consum;
    }
}
